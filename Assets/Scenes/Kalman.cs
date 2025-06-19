using System;
using MathNet.Numerics.LinearAlgebra;

public class KalmanFilter
{
    public Vector<double> X { get; private set; }

    public Matrix<double> P { get; private set; }

    public Matrix<double> F { get; set; }

    public Matrix<double> H { get; set; }

    public Matrix<double> Q { get; set; }

    public Matrix<double> R { get; set; }

    private readonly Matrix<double> _identity;
    private readonly Matrix<double> _Rbase;

    public KalmanFilter(int stateSize, int measurementSize)
    {
        var M = Matrix<double>.Build;
        var V = Vector<double>.Build;

        X = V.Dense(stateSize);
        P = M.DenseIdentity(stateSize) * 1e6;

        F = M.DenseIdentity(stateSize);
        H = M.Dense(measurementSize, stateSize);
        for (int i = 0; i < measurementSize; i++)
        {
            H[i, i] = 1.0; 
        }

        Q = M.DenseIdentity(stateSize) * 1e-4; // Process noise covariance

        _identity = M.DenseIdentity(stateSize);
        _Rbase = M.DenseIdentity(measurementSize) * 1e-2;
    }
    
    public void Predict()
    {
        X = F * X;

        P = F * P * F.Transpose() + Q;
    }

    public void Update(float x, float y, float z, double presence)
    {
        var Z = Vector<double>.Build.DenseOfArray(new double[] { x, y, z });

        var R = _Rbase.Divide(Math.Max(1e-6, presence * presence));

        var S = H * P * H.Transpose() + R;
        var PHt = P * H.Transpose();
        var K = S.Solve(PHt.Transpose()).Transpose();

        var Y = Z - (H * X);
        X = X + (K * Y);

        P = (_identity - (K * H)) * P * (_identity - (K * H)).Transpose() + K * R * K.Transpose();
    }
}