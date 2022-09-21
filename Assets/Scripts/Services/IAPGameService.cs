using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface IIAPGameService  : IService
{
    Task Initialize(Dictionary<string, string> products);
    public bool IsReady();
    Task<bool> StartPurchase(string product);

    public string GetLocalizedPrice(string product);
}
