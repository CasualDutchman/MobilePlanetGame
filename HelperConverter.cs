using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperConverter {

    public static int IntFromColor(Color col) {
        int rgb = (((int)(col.r * 256) & 0x0ff) << 16) | ((int)(col.g * 256) << 8) | ((int)(col.b * 256) & 0x0ff);
        return rgb;
    }

    public static Color ColorFromInt(int i) {
        int red = (i >> 16) & 0x0ff;
        int green = (i >> 8) & 0x0ff;
        int blue = (i) & 0x0ff;

        Color col = new Color();
        col.r = red / 256f;
        col.g = green / 256f;
        col.b = blue / 256f;
        col.a = 1;

        return col;
    }

	public static string StringFromInt(float[] values) {
        string newLine = "";
        for (int i = 0; i < values.Length; i++) {
            newLine += values[i].ToString("F2") + (i == values.Length - 1 ? "" : "/");
        }
        return newLine;
    }

    public static string AddStringFromInt(string original, int value) {
        return original + "/" + value.ToString();
    }

    public static string AddStringFromFloat(string original, float value) {
        return original + "/" + value.ToString();
    }

    public static float[] FloatFromString(string str) {
        string[] s = str.Split("/".ToCharArray());
        float[] i = new float[s.Length];
        for (int i2 = 0; i2 < s.Length; i2++) {
            i[i2] = float.Parse(s[i2]);
        }
        return i;
    }

    public static int[] IntFromString(string str) {
        string[] s = str.Split("/".ToCharArray());
        int[] i = new int[s.Length];
        for (int i2 = 0; i2 < s.Length; i2++) {
            i[i2] = Mathf.FloorToInt(float.Parse(s[i2]));
        }
        return i;
    }
}
