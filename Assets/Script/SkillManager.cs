using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    public Dash_Skill DashSkill { get; private set; }

    private void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        DashSkill = GetComponent<Dash_Skill>();
    }
}
