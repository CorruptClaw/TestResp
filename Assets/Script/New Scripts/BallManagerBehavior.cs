using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    [SerializeField] private int ballCount;
    private List<BallBehaviorTest> allBalls;
    private Dictionary<string, List<List<BallBehaviorTest>>> colorGroups = new Dictionary<string, List<List<BallBehaviorTest>>>();

    void Start()
    {
        allBalls = new List<BallBehaviorTest>(FindObjectsByType<BallBehaviorTest>(FindObjectsInactive.Exclude, FindObjectsSortMode.None));
        ballCount = allBalls.Count;

        // Initialize connected groups
        FindAllConnectedBalls();
    }

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

            foreach (BallBehaviorTest neighbor in GetNeighbors(current))
            {
                if (!visited.Contains(neighbor) && neighbor.ballColor == current.ballColor)
                {
                    stack.Push(neighbor);
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
                neighbors.Add(neighbor);
        }

        return neighbors;
    }

    public void OnPlayerBallHit(BallBehaviorTest playerBall)
    {
        // Find connected balls of the same color
        List<BallBehaviorTest> connectedBalls = FindConnectedBallsOfSameColor(playerBall);

        // Set all connected balls and the player ball's colliders to triggers
        foreach (BallBehaviorTest ball in connectedBalls)
        {
            ball.SetColliderTrigger(true);
        }

        // Set any neighboring balls of different colors to also be triggers
        foreach (BallBehaviorTest ball in connectedBalls)
        {
            SetAdjacentNonMatchingBallsToTrigger(ball);
        }

        // Set the player ball itself to trigger
        playerBall.SetColliderTrigger(true);
    }

    List<BallBehaviorTest> FindConnectedBallsOfSameColor(BallBehaviorTest ball)
    {
        List<BallBehaviorTest> connectedBalls = new List<BallBehaviorTest>();
        HashSet<BallBehaviorTest> visited = new HashSet<BallBehaviorTest>();
        FindConnectedBalls(ball, connectedBalls, visited);
        return connectedBalls;
    }

    void SetAdjacentNonMatchingBallsToTrigger(BallBehaviorTest ball)
    {
        foreach (BallBehaviorTest neighbor in GetNeighbors(ball))
        {
            if (neighbor.ballColor != ball.ballColor)
            {
                neighbor.SetColliderTrigger(true);
            }
        }
    }

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