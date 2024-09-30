

type Skill =
    {
        Id: number;
        Name: string;
        SkillLevel: number;
        Courses: number[];
    }

const Skills: Skill[] = [];

function AddSkill(skill: Skill)
{
    Skills.push(skill);
    console.log("sanyika: " + skill.Name);
}