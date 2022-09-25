using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface IGameProgressionProvider
{
    Task<bool> Initialize();
    string Load();
    void Save(string text);
}
