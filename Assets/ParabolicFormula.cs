// y = ax^2+bx+c
using System;

[Serializable]
public class ParabolicFormula {

    public float a;
    public float b;
    public float c;

    public float GetY(float x) {
        float firstBlock = a * x * x; // ax^2
        float secondBlock = b * x; // bx
        return firstBlock + secondBlock + c;
    }

}