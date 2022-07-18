using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public enum AnimationType
{
    walking,
    attacking,
    jump,
    roll,
    transition,
    idle,
}

public enum AnimationValueType
{
    trigger,
    floatValue,
    intValue,
    boolValue,
    none
}
public class AnimationManager : MonoBehaviour
{
    [SerializeField] bool useDefaultTransitions;
    public bool UseDefaultTransitions{ get => useDefaultTransitions; }
    [SerializeField] string name;
    public string Name{ get => name; }
    [SerializeField] Animator animator;
    public Animator Animator { get => animator; set => animator = value; }
    [SerializeField] AnimationFile[] files;
    public AnimationFile[] Files{ get => files; }

    /// <summary>
    /// Sets a bool for given Animation
    /// </summary>
    /// <param name="type"></param>
    /// <param name="animationName"></param>
    /// <param name="animationState"></param>
    public void SetAnimation(AnimationType type,string animationName,bool animationState)
    {
        bool foundAnimation = false;

        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].AnimationType == type && animationName == files[i].AnimationName)
            {
                for (int j = 0; j < animator.parameters.Length; j++)
                {
                    if (animator.parameters[i].name == animationName)
                    {
                        foundAnimation = true;
                        files[i].SetAnimationState(animator,animationState);
                        return;
                    }
                }
            }
        }

        if (foundAnimation == false)
        {
            Debug.LogError("WrongAnimationName");
        }
    }
    /// <summary>
    /// Sets A trigger animation
    /// </summary>
    /// <param name="type"></param>
    /// <param name="animationName"></param>
    public void SetAnimation(AnimationType type, string animationName)
    {
        bool foundAnimation = false;

        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].AnimationType == type && animationName == files[i].AnimationName)
            {
                Debug.Log("Passed");
                for (int j = 0; j < animator.parameters.Length; j++)
                {
                    if (animator.parameters[j].name == animationName)
                    {
                        files[i].SetAnimationState(animator);
                        foundAnimation = true;
                        Debug.Log(foundAnimation);
                        return;
                    }
                }
            }
        }

        if (foundAnimation == false)
        {
            Debug.LogError("WrongAnimationName");
        }
    }
    /// <summary>
    /// Sets a integer value for the given animation
    /// </summary>
    /// <param name="type"></param>
    /// <param name="animationName"></param>
    /// <param name="setValue"></param>
    public void SetAnimation(AnimationType type,string animationName,int setValue)
    {
        bool foundAnimation = false;

        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].AnimationType == type && animationName == files[i].AnimationName)
            {
                for (int j = 0; j < animator.parameters.Length; j++)
                {
                    if (animator.parameters[i].name == animationName)
                    {
                        foundAnimation = true;
                        files[i].SetAnimationState(animator,setValue);
                        return;
                    }
                }
            }
        }

        if (foundAnimation == false)
        {
            Debug.LogError("WrongAnimationName");
        }
    }
    /// <summary>
    /// Sets an float value for the given animation
    /// </summary>
    /// <param name="type"></param>
    /// <param name="animationName"></param>
    /// <param name="setValue"></param>
    public void SetAnimation(AnimationType type, string animationName, float setValue)
    {
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].AnimationType == type && animationName == files[i].AnimationName)
            {
                for (int j = 0; j < animator.parameters.Length; j++)
                {
                    if (animator.parameters[i].name == animationName)
                    {
                        files[i].SetAnimationState(animator, setValue);
                        return;
                    }
                    else
                    {
                        Debug.LogError("WrongAnimationName");
                    }
                }
            }
        }
    }
}

[System.Serializable]
public class AnimationFile
{
    [SerializeField] AnimationValueType valueType;
    public AnimationValueType ValueType{ get => valueType; }
    [SerializeField] AnimationType animationType;
    public AnimationType AnimationType { get => animationType; }
    [SerializeField] string animationName;
    public string AnimationName{ get => animationName; }
    [SerializeField] Motion animation;
    public Motion Animation{ get => animation; }
    [SerializeField] bool hasExitTime;
    public bool HasExitTime{ get => hasExitTime; }

    /// <summary>
    /// Sets a bool for the animation
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="animationState"></param>
    public void SetAnimationState(Animator animator,bool animationState)
    {
        animator.SetBool(animationName,true);
    }
    /// <summary>
    /// Sets a trigger
    /// </summary>
    /// <param name="animator"></param>
    public void SetAnimationState(Animator animator)
    {
        animator.SetTrigger(animationName);
    }
    /// <summary>
    /// Sets a animationValue
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="value"></param>
    public void SetAnimationState(Animator animator,int value)
    {
        animator.SetInteger(animationName,value);
    }
    /// <summary>
    /// Sets float value for a animation
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="value"></param>
    public void SetAnimationState(Animator animator, float value)
    {
        animator.SetFloat(animationName, value);
    }
}
