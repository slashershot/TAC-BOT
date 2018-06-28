﻿// Decompiled with JetBrains decompiler
// Type: LightMultipler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FE644F5D-682F-4D6E-964D-A0DD77A288F7
// Assembly location: C:\Users\André\Desktop\Assembly-CSharp.dll

using UnityEngine;

[DisallowMultipleComponent]
[ExecuteInEditMode]
[AddComponentMenu("Rendering/Light Multipler")]
public class LightMultipler : MonoBehaviour
{
  public float Radius;
  public float Exponent;
  public Vector3 Multipler;
  protected Vector3 mPositionCache;

  public LightMultipler()
  {
    base.\u002Ector();
  }

  public virtual void Cache()
  {
    this.mPositionCache = ((Component) this).get_transform().get_position();
  }

  protected Vector3 CalcMultipler(float distance)
  {
    return Vector3.Lerp(Vector3.get_one(), this.Multipler, Mathf.Pow(Mathf.Clamp01((float) (1.0 - (double) distance / (double) this.Radius)), this.Exponent));
  }

  public virtual Vector3 CalcMultiplerAtPoint(Vector3 position, Vector3 normal)
  {
    if ((double) this.Radius <= 0.0)
      return Vector3.get_one();
    Vector3 vector3 = Vector3.op_Subtraction(position, this.mPositionCache);
    // ISSUE: explicit reference operation
    return this.CalcMultipler(((Vector3) @vector3).get_magnitude());
  }

  private void Awake()
  {
    ((Component) this).set_tag("EditorOnly");
    ((Behaviour) this).set_enabled(false);
  }
}