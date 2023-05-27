using UnityEngine;
using UnityEditor;

public class MonsterGoblinWindow : EditorWindow
{
    private GameObject prefab;
    //private MonsterManager mgr;
    //private MonsterData data;

    [MenuItem("Monster/Monster Information /Goblin %g")]
    private static void OpenGoblinWindow()
    {
        MonsterGoblinWindow window = GetWindow<MonsterGoblinWindow>("Goblin Window");
        window.minSize = new Vector2(500, 700);
        window.maxSize = window.minSize;
        window.Show();
    }

    private void Awake()
    {
        prefab = GameObject.Find("goblin");
        //mgr = findob
    }
    private void OnGUI()
    {
        GUI.Box(new Rect(0, 100, Screen.width, Screen.height / 2), "");
        prefab = EditorGUI.ObjectField(new Rect(0, 75, position.width - 400, 25), "", prefab, typeof(GameObject), true) as GameObject;

        if (prefab != null)
        {
            Texture2D previewTexture = AssetPreview.GetAssetPreview(prefab);
            if (previewTexture != null)
            {
                Rect previewRect = new Rect(100, 100, Screen.width - 200, Screen.height / 2);
                EditorGUI.DrawPreviewTexture(previewRect, previewTexture);
            }
        }

        GUIStyle titleLabel = new GUIStyle(GUI.skin.label);
        titleLabel.fontSize = 24;
        titleLabel.alignment = TextAnchor.UpperCenter;

        //타이틀 제목
        GUI.Label(new Rect(100, 25, Screen.width - 200, Screen.height / 2), "고블린" /*+ prefab.name*/, titleLabel);
        //몬스터 정보
        GUI.Label(new Rect(100, 470, Screen.width - 200, Screen.height), "Goblin Information" /*+ prefab.name*/, titleLabel);


        GUIStyle textLabel = new GUIStyle(GUI.skin.label);
        textLabel.fontSize = 18;
        textLabel.fixedHeight = 70;
        textLabel.fixedWidth = 500;

        GUIStyle textArea = new GUIStyle(GUI.skin.label);
        textArea.fontSize = 16;
        textArea.fixedHeight = 50;
        textArea.fixedWidth = 200;

        //Data 체력
        GUI.Label(new Rect(0,500, 200, 100),"체력:\t\ + ", textLabel);

        GUI.Label(new Rect(0, 550, 200, 100), "방어력:\t\t20", textLabel);

        GUI.Label(new Rect(0, 600, 200, 100), "공격력:\t\t20", textLabel);

        GUI.Label(new Rect(0, 650, 200, 100), "이동속도:\t\t10", textLabel);

        GUI.Label(new Rect(220, 500, 200, 150), "패시브 :\n체력 자동 회복(비 전투시)", textLabel);

        GUI.Label(new Rect(220, 600, 200, 100), "특징 :\n 체력이 회복되므로\n~~~해야합니다", textLabel);

    }
}