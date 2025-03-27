using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGenericController
{
    // Start is called before the first frame update
    void Activate();
    void Reset();
    void Deactivate();
}
