using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    [SerializeField] private int ballCount;
    public List<BallBehaviorTest> allBalls;
    private Dictionary<string, List<List<BallBehaviorTest>>> colorGroups = new Dictionary<string, List<List<BallBehaviorTest>>>();

    void Start()
    {
        allBalls = new List<BallBehaviorTest>(FindObjectsByType<BallBehaviorTest>(FindObjectsInactive.Exclude, FindObjectsSortMode.None));
        ballCount = allBalls.Count;

        Debug.Log($"Total balls found: {ballCount}");
        // Initialize connected groups
        FindAllConnectedBalls();
    }

    #region Check and find balls(with same color) on the level and connect them
    void FindAllConnectedBalls()
    {
        HashSet<BallBehaviorTest> visited = new HashSet<BallBehaviorTest>();

        foreach (BallBehaviorTest ball in allBalls)
        {
            if (!visited.Contains(ball))
            {
                List<BallBehaviorTest> connectedBalls = new List<BallBehaviorTest>();
                FindConnectedBalls(ball, connectedBalls, visited);

                if (!colorGroups.ContainsKey(ball.ballColor))
                {
                    colorGroups[ball.ballColor] = new List<List<BallBehaviorTest>>();
                }
                colorGroups[ball.ballColor].Add(connectedBalls);
                //Debug.Log($"New group found for color {ball.ballColor} with {connectedBalls.Count} balls.");
            }
        }

        PrintGroups();
    }

    void FindConnectedBalls(BallBehaviorTest ball, List<BallBehaviorTest> connectedBalls, HashSet<BallBehaviorTest> visited)
    {
        Stack<BallBehaviorTest> stack = new Stack<BallBehaviorTest>();
        stack.Push(ball);

        while (stack.Count > 0)
        {
            BallBehaviorTest current = stack.Pop();

            if (visited.Contains(current)) continue;
            visited.Add(current);
            connectedBalls.Add(current);

            //Debug.Log($"Added Ball at {current.position} to connected group (color {current.ballColor})");

            foreach (BallBehaviorTest neighbor in GetNeighbors(current))
            {
                if (!visited.Contains(neighbor) && neighbor.ballColor == current.ballColor)
                {
                    stack.Push(neighbor);
                    //Debug.Log($"Neighbor at {neighbor.position} with color {neighbor.ballColor} is connected to {current.position}");
                }
            }
        }
    }

    List<BallBehaviorTest> GetNeighbors(BallBehaviorTest ball)
    {
        List<BallBehaviorTest> neighbors = new List<BallBehaviorTest>();
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        foreach (Vector2Int direction in directions)
        {
            Vector2Int neighborPosition = ball.position + direction;
            BallBehaviorTest neighbor = allBalls.Find(b => b.position == neighborPosition);
            if (neighbor != null)
            {
                neighbors.Add(neighbor);
                //Debug.Log($"Neighbor found at {neighborPosition} for ball at {ball.position}");
            }

        }

        return neighbors;
    }
#endregion
    #region How the player ball and the ball that is connected behaves
    public void OnPlayerBallHit(BallBehaviorTest playerBall)
    {
        //Debug.Log($"Player ball hit detected at {playerBall.position} with color {playerBall.ballColor}");

        // Find connected balls of the same color
        List<BallBehaviorTest> connectedBalls = FindConnectedBallsOfSameColor(playerBall);

        // Only proceed if there are 3 or more connected balls of the same color
        if (connectedBalls.Count >= 3)
        {
            Debug.Log($"Group of {connectedBalls.Count} balls of color {playerBall.ballColor} meets the threshold. Changing colliders to triggers.");

            // Set all connected balls and the player ball's colliders to triggers
            foreach (BallBehaviorTest ball in connectedBalls)
            {
                ball.SetColliderTrigger(true); // Set collider to trigger to make it "fall"
                //playerBall.SetColliderTrigger(true);
            }

            // Set the player ball itself to trigger
            playerBall.SetColliderTrigger(true);
        }
        else
        {
            Debug.Log($"Group of {connectedBalls.Count} balls of color {playerBall.ballColor} is too small to trigger. No changes made.");
        }

    }

    List<BallBehaviorTest> FindConnectedBallsOfSameColor(BallBehaviorTest ball)
    {
        List<BallBehaviorTest> connectedBalls = new List<BallBehaviorTest>();
        HashSet<BallBehaviorTest> visited = new HashSet<BallBehaviorTest>();

        FindConnectedBalls(ball, connectedBalls, visited);
        //Debug.Log($"Total connected balls found for color {ball.ballColor}: {connectedBalls.Count}");

        return connectedBalls;
    }

#endregion
    void PrintGroups()
    {
        foreach (var colorGroup in colorGroups)
        {
            Debug.Log($"Color: {colorGroup.Key}");
            foreach (var group in colorGroup.Value)
            {
                string groupBalls = "";
                foreach (BallBehaviorTest ball in group)
                {
                    groupBalls += $"Ball at {ball.position} ";
                }
                Debug.Log($"Connected Group: {groupBalls}");
            }
        }
    }
}