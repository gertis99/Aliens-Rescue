using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapCellsAnimation : IAnimation
{
    private GameObject object1, object2;

    public SwapCellsAnimation(GameObject obj1, GameObject obj2)
    {
        object1 = obj1;
        object2 = obj2;
    }

    public Coroutine PlayAnimation(GridView board)
    {
        return board.StartCoroutine(AnimationCoroutine(board));
    }

    private IEnumerator AnimationCoroutine(GridView board)
    {
        object1.transform.DOMove(new Vector3(object2.transform.position.x, object2.transform.position.y, 0), 0.5f).SetEase(Ease.InOutQuad);
        object2.transform.DOMove(new Vector3(object1.transform.position.x, object1.transform.position.y, 0), 0.5f).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(0.5f);
    }
}
