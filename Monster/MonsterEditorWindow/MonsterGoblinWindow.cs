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

        //Ÿ��Ʋ ����
        GUI.Label(new Rect(100, 25, Screen.width - 200, Screen.height / 2), "���" /*+ prefab.name*/, titleLabel);
        //���� ����
        GUI.Label(new Rect(100, 470, Screen.width - 200, Screen.height), "Goblin Information" /*+ prefab.name*/, titleLabel);


        GUIStyle textLabel = new GUIStyle(GUI.skin.label);
        textLabel.fontSize = 18;
        textLabel.fixedHeight = 70;
        textLabel.fixedWidth = 500;

        GUIStyle textArea = new GUIStyle(GUI.skin.label);
        textArea.fontSize = 16;
        textArea.fixedHeight = 50;
        textArea.fixedWidth = 200;

        //Data ü��
        GUI.Label(new Rect(0,500, 200, 100),"ü��:\t\ + ", textLabel);

        GUI.Label(new Rect(0, 550, 200, 100), "����:\t\t20", textLabel);

        GUI.Label(new Rect(0, 600, 200, 100), "���ݷ�:\t\t20", textLabel);

        GUI.Label(new Rect(0, 650, 200, 100), "�̵��ӵ�:\t\t10", textLabel);

        GUI.Label(new Rect(220, 500, 200, 150), "�нú� :\nü�� �ڵ� ȸ��(�� ������)", textLabel);

        GUI.Label(new Rect(220, 600, 200, 100), "Ư¡ :\n ü���� ȸ���ǹǷ�\n~~~�ؾ��մϴ�", textLabel);

    }
}