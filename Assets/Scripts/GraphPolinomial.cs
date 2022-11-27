using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]

public class GraphPolinomial : MonoBehaviour
{
    public Vector3[] points;
    public float xMin = 0;
    public float xMax = 10;
    public float spacing = 0.1f;
    
    private int pointCount;
    private LineRenderer line;
    public Vector3 CalculatePoints(float x, float[] coefficients)
    {
        float y = 0;
        for (int i = 0; i < coefficients.Length; i++)
        {
            y += coefficients[i] * Mathf.Pow(x, i);
        }
        return new Vector3(x, y, 0);
    }

    private void Start() {
        
        
        line = GetComponent<LineRenderer>();

        //Obtiene la cantidad de puntos entre el principio y el final del eje x para
        //Graficar el polinomio
        line.positionCount = (int)((xMax - xMin) / spacing);

    }

    private void Update() {

        //Get polinoimal coefficients
        float[] coefficients = GetPolinomial(points);

        //Calculate points
        for (int i = 0; i < line.positionCount; i++)
        {
            float x = xMin + i * spacing;
            line.SetPosition(i, CalculatePoints(x, coefficients));
        }
    }

    // Get coefficients of the polinomial that passes through the points using the Vandermonde matrix
    public float[] GetPolinomial(Vector3[] points)
    {
        float[] coefficients = new float[points.Length];

        // Create Vandermonde matrix
        float[,] vandermonde = new float[points.Length, points.Length];
        for (int i = 0; i < points.Length; i++)
        {
            for (int j = 0; j < points.Length; j++)
            {
                vandermonde[i, j] = Mathf.Pow(points[i].x, j);
            }
        }

        // Create vector of y values
        float[] y = new float[points.Length];
        for (int i = 0; i < points.Length; i++)
        {
            y[i] = points[i].y;
        }

        // Solve for coefficients
        coefficients = SolveLinearSystem(vandermonde, y);

        return coefficients;
    }

    // Solve linear system using Gaussian elimination
    private float[] SolveLinearSystem(float[,] A, float[] b)
    {
        int n = b.Length;

        // Forward elimination
        for (int i = 0; i < n; i++)
        {
            // Search for maximum in this column
            float maxEl = Mathf.Abs(A[i, i]);
            int maxRow = i;
            for (int k = i + 1; k < n; k++)
            {
                if (Mathf.Abs(A[k, i]) > maxEl)
                {
                    maxEl = Mathf.Abs(A[k, i]);
                    maxRow = k;
                }
            }

            // Swap maximum row with current row (column by column)
            for (int k = i; k < n; k++)
            {
                float tmp = A[maxRow, k];
                A[maxRow, k] = A[i, k];
                A[i, k] = tmp;
            }
            // Swap maximum row with current row (b by b)
            float tmp2 = b[maxRow];
            b[maxRow] = b[i];
            b[i] = tmp2;

            // Make all rows below this one 0 in current column
            for (int k = i + 1; k < n; k++)
            {
                float c = -A[k, i] / A[i, i];
                for (int j = i; j < n; j++)
                {
                    if (i == j)
                    {
                        A[k, j] = 0;
                    }
                    else
                    {
                        A[k, j] += c * A[i, j];
                    }
                }
                b[k] += c * b[i];
            }
        }

        // Solve equation Ax=b for an upper triangular matrix A
        float[] x = new float[n];
        for (int i = n - 1; i >= 0; i--)
        {
            x[i] = b[i] / A[i, i];
            for (int k = i - 1; k >= 0; k--)
            {
                b[k] -= A[k, i] * x[i];
            }
        }
        return x;
    }
}
