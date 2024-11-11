using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManagerBehavior : MonoBehaviour
{
    public List<BallBehaviorTest> allBalls;
    private Dictionary<string, List<List<BallBehaviorTest>>> colorGroups;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colorGroups = new Dictionary<string, List<List<BallBehaviorTest>>>();
        FindAllConnectedBalls();
    }

    // Update is called once per frame
    void Update()
    {
        
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
            {
                neighbors.Add(neighbor);
            }
        }

        return neighbors;
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


