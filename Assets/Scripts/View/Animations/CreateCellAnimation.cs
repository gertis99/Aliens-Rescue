using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCellAnimation : IAnimation
{
    private Vector2Int position;
    private GameObject objectAnimated;

    public CreateCellAnimation(Vector2Int pos, GameObject obj)
    {
        position = pos;
        objectAnimated = obj;
    }

    public Coroutine PlayAnimation(GridView board)
    {
        return board.StartCoroutine(AnimationCoroutine(board));
    }

    private IEnumerator AnimationCoroutine(GridView board)
    {
        objectAnimated.SetActive(true);
        objectAnimated.transform.DOMove(new Vector3(position.x, position.y, 0), 0.05f).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(0.05f);
    }
}
