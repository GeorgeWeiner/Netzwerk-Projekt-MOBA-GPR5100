using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

[CustomEditor(typeof(AnimationManager),true)]
public class AnimationManagerEditor : Editor
{
    private Animator animator;
    private AnimationManager animationManager;
    private const string savePath = "Assets/Meshes & Animations/Animator";

    private void Awake()
    {
        animationManager = (AnimationManager)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("AddAnimations"))
        {
            AddAnimations();
        }
    }

    public void AddAnimations()
    {
        animationManager = (AnimationManager)target;
        string savePath = AnimationManagerEditor.savePath + animationManager.Name + ".controller";

        AnimatorController controller = AnimatorController.CreateAnimatorControllerAtPath(savePath);
        AnimationFile[] files = animationManager.Files;
        AnimatorState[] animations = new AnimatorState[files.Length];

        for (int i = 0; i < animationManager.Files.Length; i++)
        {
            switch (files[i].ValueType)
            {
                case AnimationValueType.boolValue:
                    controller.AddParameter(files[i].AnimationName, AnimatorControllerParameterType.Bool);
                    animations[i] = controller.AddMotion(files[i].Animation, 0);
                    break;
                case AnimationValueType.floatValue:
                    controller.AddParameter(files[i].AnimationName, AnimatorControllerParameterType.Float);
                    animations[i] = controller.AddMotion(files[i].Animation, 0);
                    break;
                case AnimationValueType.intValue:
                    controller.AddParameter(files[i].AnimationName, AnimatorControllerParameterType.Int);
                    animations[i] = controller.AddMotion(files[i].Animation, 0);
                    break;
                case AnimationValueType.trigger:
                   
                    controller.AddParameter(files[i].AnimationName, AnimatorControllerParameterType.Trigger);
                    animations[i] = controller.AddMotion(files[i].Animation, 0);
                    break;
            }
        }
        if (animationManager.UseDefaultTransitions)
        {
            foreach (var animatorState in animations)
            {
                for (int i = 0; i < animations.Length; i++)
                {
                    if (animatorState != animations[i] && animatorState.name != files[i].AnimationName)
                    {
                        animatorState.AddTransition(animations[i], files[i].HasExitTime);
                    }
                }
            }
        }

        if (animationManager.gameObject.GetComponent<Animator>() != null)
        {
            animationManager.gameObject.GetComponent<Animator>().runtimeAnimatorController = (RuntimeAnimatorController)AssetDatabase.LoadAssetAtPath(AssetDatabase.GetAssetPath(controller), typeof(AnimatorController));
            animationManager.Animator = animationManager.GetComponent<Animator>();
        }
        else
        {
            var animator = animationManager.gameObject.AddComponent<Animator>().runtimeAnimatorController;
            animator = (RuntimeAnimatorController)AssetDatabase.LoadAssetAtPath(AssetDatabase.GetAssetPath(controller), typeof(AnimatorController));
            animationManager.Animator = animationManager.GetComponent<Animator>();
        }
    }
}
