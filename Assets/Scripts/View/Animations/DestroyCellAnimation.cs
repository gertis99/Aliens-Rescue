using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DestroyCellAnimation : IAnimation
{
    private GameObject objectAnimated;

    public DestroyCellAnimation(GameObject obj)
    {
        objectAnimated = obj;
    }

    public Coroutine PlayAnimation(GridView board)
    {
        return board.StartCoroutine(AnimationCoroutine(board));
    }

    private IEnumerator AnimationCoroutine(GridView board)
    {
        DOTween.Sequence(objectAnimated.transform.DOScale(0f, 0.5f));
        yield return new WaitForSeconds(0.5f);
        board.DestroyCell(objectAnimated);
    }
}
