using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Door : MonoBehaviour
{
    public static Door Instance;
    public Item Slot1, Slot2;
    public RectTransform Star_Anim,Star_UI;
    public Transform throwTowards;
   

    [Range(0f,1.0f)]
    public float value = 1;
    public Transform leftDoor, rightDoor;
    public Transform leftPosi, rightPosi;

    private bool isCollectiong = false;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (!leftDoor || !rightDoor) return;
        leftDoor.transform.localPosition = new Vector3(-value/3, 0,0);
        rightDoor.transform.localPosition = new Vector3(value/3, 0,0);
    }



    private void OnTriggerStay(Collider other)
    {
        if (isCollectiong) return;
        Item tmp = null;
        try
        {
            tmp = other.GetComponent<Item>();
        }
        catch (System.Exception)
        {

        }
        if (!tmp || Slot1 == tmp || Slot2 == tmp) return;

        if (Slot1 == null)
        {
            Slot1 = tmp;
            //place
            PlaceItem(tmp,leftPosi);
        }
        else
        {
            if (Slot1.Name == tmp.Name)
            {
                Slot2 = tmp;
                //place
                PlaceItem(tmp, rightPosi);
            }
            else
            {
                Slot1.transform.DOShakePosition(0.5f,0.05f);
                Slot1.transform.DOShakeScale(0.5f, 0.05f);
                Throw(tmp);
            }
        }

        if (Slot1 != null && Slot2 != null) StartCoroutine(AnimateCollect());
    }


    public void PlaceItem(Item obj,Transform posi)
    {
       obj.Placed(true);
       StartCoroutine(MoveToPosition(obj,posi.position,0.5f));
       StartCoroutine(RemoveRotation(obj.transform, 0.5f));
    }
    public void Throw(Item obj)
    {
        obj.Placed(false);
        StartCoroutine(ThrowToCenter(obj,new Vector3(-50,-1,2)));
    }
    IEnumerator ThrowToCenter(Item _obj, Vector3 posi)
    {
        Debug.Log("Throw");
        yield return new WaitForFixedUpdate();
        _obj.transform.localEulerAngles = Vector3.zero;
        _obj.rb.velocity = Vector3.zero;
        yield return new WaitForFixedUpdate();
        _obj.rb.AddForce((throwTowards.position - transform.position)*1.5f,ForceMode.VelocityChange);
       // _obj.rb.isKinematic = false;
       // _obj.rb.useGravity = true;
      
       // Debug.Log(_obj.rb.velocity);
       // _obj.rb.AddForce((posi - _obj.transform.position) * 50);
       // Debug.Log(_obj.rb.velocity);
    }

    IEnumerator MoveToPosition(Item obj, Vector3 position, float timeToMove)
    {        
        var currentPos = obj.transform.position;
        var t = 0f;
        while (t < 0.5f)
        {
            t += Time.deltaTime / timeToMove;
            obj.transform.position = Vector3.Lerp(currentPos, position, t);
            yield return null;
        }
        obj.transform.position = position;
        obj.transform.localEulerAngles = Vector3.zero;
    }
    IEnumerator AnimateCollect()
    {
        isCollectiong = true;
        Item s1 = Slot1;
        Item s2 = Slot2;
        Slot1  = null;
        Slot2  = null;
        s1.myCol.enabled = false;
        s2.myCol.enabled = false;
        s1.rb.isKinematic = true;
        s2.rb.isKinematic = true;
        yield return new WaitForSeconds(0.25f);
        AddOverTime(0.5f, 1, (a) => value += a);

        yield return new WaitForSeconds(0.4f);

        s1.transform.DOMove(transform.position,0.2f);
        s2.transform.DOMove(transform.position,0.2f);        
        s1.transform.DOScale(Vector3.zero,.3f);
        s2.transform.DOScale(Vector3.zero,.3f);

        GameManager.Instance.m_liveObject.RemoveAt(0);
        GameManager.Instance.m_liveObject.RemoveAt(0);

        yield return new WaitForSeconds(0.2f);
        Star_Anim.gameObject.SetActive(true);
        Star_Anim.localPosition= new Vector2(0,-550);
        Star_Anim.localScale = Vector3.zero;
        Star_Anim.DOScale(new Vector3(3f, 3f, 3f), 1f);        
       // str.DOMove();
        yield return new WaitForSeconds(1f);
        AddOverTime(0.5f, 1, (a) => value -= a);
        //move star 
        Star_Anim.DOScale(new Vector3(1f, 1f, 1f), 0.75f);
        Star_Anim.DOMove(Star_UI.transform.position, 1f);
        yield return new WaitForSeconds(1f);
        Star_Anim.gameObject.SetActive(false);
        StorageManager.instance.levelStar++;
        StorageManager.instance.TotalScore++;
        GameManager.Instance.stars_txt.text = StorageManager.instance.levelStar.ToString();
        //StorageManager.instance.CollectingPair++;
        GameManager.Instance.myPool.Despawn(s1.transform);
        GameManager.Instance.myPool.Despawn(s2.transform);
        StorageManager.instance.CollectingPair++;
        isCollectiong = false;
    }
    IEnumerator RemoveRotation(Transform transform, float timeToMove)
    {
        var currentRot = transform.rotation;
        var targetRot = Quaternion.Euler(0f, 180f, 0f);
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            transform.rotation = Quaternion.Slerp(currentRot, targetRot, t);
            yield return null;
        }
    }
    IEnumerator _AddOverTime(float aDuration, float aAmount, System.Action<float> aCallback)
    {
        float t = 0f;
        float step = aAmount / aDuration;
        while (t < aAmount)
        {
            float add = step * Time.deltaTime;
            if (add >= aAmount - t)
            {
                aCallback(aAmount - t);
                yield break;
            }
            aCallback(add);
            t += add;
            yield return null;
        }
    }
    public void AddOverTime(float aDuration, float aAmount, System.Action<float> aCallback)
    {
        StartCoroutine(_AddOverTime(aDuration, aAmount, aCallback));
    }
}
