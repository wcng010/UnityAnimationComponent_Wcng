using System;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using System.Collections;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using System.Collections.Generic;
using GameScript.GameMain.Skill;
using UnityEditor.SceneManagement;
using Unity.EditorCoroutines.Editor;
using UnityEngine.SceneManagement;

namespace Wcng.SkillEditor
{
    public class SkillEditorWindow : EditorWindow
    {
        public static SkillEditorWindow Instance { get; private set; }

        private VisualElement root;

        private const string stylePath = "Assets/GameScript/GameEditor/SkillEditor/Extend.uss";
        private const string AssetPath = "Assets/GameScript/GameEditor/SkillEditor/SkillEiditorWindow.uxml";

       [MenuItem("SkillEditor/SkillEiditorWindow")]
        public static void ShowExample()
        {
            SkillEditorWindow wnd = GetWindow<SkillEditorWindow>();
            wnd.titleContent = new GUIContent("技能编辑器 ");
        }

        private void OnDestroy()
        {
            if (skillConfig != null)
                AutoSaveConfig();
        }

        public void CreateGUI()
        {
            SkillConfig.SetSkillValidateAction(ResetView);

            Instance = this;

            //UIElement的    
            root = rootVisualElement;
            StyleSheet uss = AssetDatabase.LoadAssetAtPath<StyleSheet>(stylePath);
            root.styleSheets.Add(uss);
            // Import UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(AssetPath);
            VisualElement labelFromUXML = visualTree.Instantiate();
            root.Add(labelFromUXML);
            
            InitTopMenu();
            InitTimeShaft();
            InitContent();
            InitConsole();

            //配置
            if (skillConfig != null)
            {
                CurrentFrameCount = skillConfig.FrameCount;
            }
            else { CurrentFrameCount = 100; }

            CurrentSelectFrameIndex = 0;

            //刷新面板
            CurrentFrameTextField.value = CurrentSelectFrameIndex;
            FrameCountTextFiled.value = CurrentFrameCount;
        }

        public void ResetView()
        {
            SkillConfig tempSkillConfig = skillConfig;
            SkillConfigObjectField.value = null;
            SkillConfigObjectField.value = tempSkillConfig;
        }

        #region Config

        private SkillConfig skillConfig;

        public SkillConfig SkillConfig
        {
            get
            {
                return skillConfig;
            }
        }

        private SkillEditorConfig skillEditorConfig = new SkillEditorConfig();

        //当前选中帧
        private int currentSelectFrameIndex = -1;

        //当前选中帧
        public int CurrentSelectFrameIndex
        {
            get => currentSelectFrameIndex;
            private set
            {
                if (value != currentSelectFrameIndex)
                {
                    currentSelectFrameIndex = Mathf.Clamp(value, 0, CurrentFrameCount);
                    //面板同步
                    CurrentFrameTextField.value = currentSelectFrameIndex;
                    UpdateTimeShaftView();
                    TickSkill();
                }
            }
        }

        //总帧数
        private int currentFrameCount;

        //总帧数
        public int CurrentFrameCount
        {
            get => currentFrameCount;
            set
            {
                if (value != currentFrameCount)
                {
                    currentFrameCount = value;
                    //同步面板
                    FrameCountTextFiled.value = value;
                    //同步给skillConfig
                    if (skillConfig != null) { skillConfig.FrameCount = value; }
                    //时间轴发送变化ContentView缩放
                    UpdateContentSise();
                }
            }
        }

        #endregion Config

        #region 顶部

        private string oldScenePath;

        private ToolbarButton SkillBasicButton;

        private ToolbarButton CreateSkillTrackButton;

        private ObjectField PreviewCharacterPrefabObjectField;
        private ObjectField SkillConfigObjectField;
        private Label SkillConfigNameLabel;
        private Label PreviewCharacterGONameLabel;
        private ToolbarButton SaveConfigButton;
        private Toggle AutoSaveToggle;

        private GameObject currentPreviewGameObject;
        public GameObject CurrentPreviewGameObject { get => currentPreviewGameObject; }

        private void InitTopMenu()
        {   //预览物体txt
            PreviewCharacterGONameLabel = root.Q<Label>(nameof(PreviewCharacterGONameLabel));
            //技能配置文件txt
            SkillConfigNameLabel = root.Q<Label>(nameof(SkillConfigNameLabel));
            //技能配置按钮
            SkillBasicButton = root.Q<ToolbarButton>(nameof(SkillBasicButton));
            //技能配置添加订阅事件
            SkillBasicButton.clicked += SkillBasicButtonClick;

            //预览物体装入
            PreviewCharacterPrefabObjectField = root.Q<ObjectField>(nameof(PreviewCharacterPrefabObjectField));
            //预制体装入事件
            PreviewCharacterPrefabObjectField.RegisterValueChangedCallback(PreviewCharacterPrefabObjectFieldRegister);

            //技能Config装入
            SkillConfigObjectField = root.Q<ObjectField>(nameof(SkillConfigObjectField));
            SkillConfigObjectField.objectType = typeof(SkillConfig);
            //技能装入事件
            SkillConfigObjectField.RegisterValueChangedCallback(SkillConfigObjectFieldRegister);

            //保存按钮
            SaveConfigButton = root.Q<ToolbarButton>(nameof(SaveConfigButton));
            //保存事件
            SaveConfigButton.clicked += SaveConfig;
            //订阅自动保存事件
            AutoSaveToggle = root.Q<Toggle>(nameof(AutoSaveToggle));
            AutoSaveToggle.RegisterValueChangedCallback(AutoSaveValueChanged);
            //管理轨道创建
            CreateSkillTrackButton = root.Q<ToolbarButton>(nameof(CreateSkillTrackButton));
            CreateSkillTrackButton.clicked += OpenCreateTrackWindow;
            
            if (skillConfig != null)
            {
                SkillConfigNameLabel.text = skillConfig.name;
            }
            if (currentPreviewGameObject != null)
            {
                PreviewCharacterGONameLabel.text = CurrentPreviewGameObject.name;
            }
        }

        private void OpenCreateTrackWindow()
        {
            CreateSkillTrackWindow.ShowExample();
            CreateSkillTrackWindow.Instance.Init(skillConfig);
        }

        /// <summary>
        /// 自动保存
        /// </summary>
        /// <param name="evt"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void AutoSaveValueChanged(ChangeEvent<bool> evt)
        {
            skillEditorConfig.isAutoSaveConfig = evt.newValue;
        }

        /// <summary>
        /// 技能配置文件选择事件
        /// </summary>
        /// <param name="evt"></param>
        private void SkillConfigObjectFieldRegister(ChangeEvent<UnityEngine.Object> evt)
        {
            if (evt.newValue != evt.previousValue)
            {
                skillConfig = evt.newValue as SkillConfig;
                CurrentSelectFrameIndex = 0;
                UpdateTimeShaftView();
                if (SkillConfig != null)
                {
                    CurrentFrameCount = skillConfig.FrameCount;
                    FPSDropDownField.value = skillConfig.FrameRate.ToString();
                    SkillConfigNameLabel.text = skillConfig.skillName;
                }
                else
                {
                    CurrentFrameCount = 100;
                }
                //初始化轨道
                InitTrack();
            }
        }

        /// <summary>
        /// 角色预制体选择事件
        /// </summary>
        /// <param name="evt"></param>
        private void PreviewCharacterPrefabObjectFieldRegister(ChangeEvent<UnityEngine.Object> evt)
        {
            //避免其他场景
            if (evt.newValue != evt.previousValue)
            {
                currentPreviewGameObject = evt.newValue as GameObject;
            }
        }

        /// <summary>
        /// 回归旧场景
        /// </summary>
        private void LoadOldSceneButtonClick()
        {
            if (!string.IsNullOrEmpty(oldScenePath))
            {
                string current = EditorSceneManager.GetActiveScene().path;
                if (current != oldScenePath)
                {
                    EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                    EditorSceneManager.OpenScene(oldScenePath);
                }
            }
            else
            {
                Debug.LogWarning("场景不存在!");
            }
        }

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="evt"></param>
        private void LoadScene(ChangeEvent<string> evt)
        {
            if (evt.newValue != evt.previousValue)
            {
                //非同一个场景
                if (evt.newValue != evt.previousValue && evt.newValue != "")
                {
                    foreach (UnityEditor.EditorBuildSettingsScene S in UnityEditor.EditorBuildSettings.scenes)
                    {
                        string scenesName = S.path;
                        string[] Name = scenesName.Split('/');
                        foreach (var item in Name)
                        {
                            if (item.Contains(evt.newValue))
                            {
                                EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                                EditorSceneManager.OpenScene(scenesName);
                                oldScenePath = evt.newValue;
                                return;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 查看技能信息
        /// </summary>
        private void SkillBasicButtonClick()
        {
            if (skillConfig != null)
            {
                //unity聚焦
                Selection.activeObject = skillConfig;
            }
        }

        #endregion 顶部菜单

        #region 时间轴

        private IMGUIContainer timeShaft;
        private IMGUIContainer selectLine;
        private VisualElement contentContainer;
        private VisualElement contentViewPort;

        private bool timeShaftIsMouseEnter;//鼠标进入时间轴

        private float contentOffsetPos { get => Mathf.Abs(contentContainer.transform.position.x); }//滚动条偏移坐标
        private float currentSelectFramePos { get => CurrentSelectFrameIndex * skillEditorConfig.frameUniWidth; }//选中帧的x坐标

        private void InitTimeShaft()
        {
            ScrollView mainContentView = root.Q<ScrollView>("MainContentView");
            contentContainer = mainContentView.Q<VisualElement>("unity-content-container");
            contentViewPort = mainContentView.Q<VisualElement>("unity-content-viewport");

            timeShaft = root.Q<IMGUIContainer>("TimeShaft");
            timeShaft.onGUIHandler = DrawTimeShaft;//绘制时间轴
            timeShaft.RegisterCallback<WheelEvent>(TimeShaftWheel);
            timeShaft.RegisterCallback<MouseDownEvent>(TimeShaftMouseDownEvent);
            timeShaft.RegisterCallback<MouseMoveEvent>(TimeShaftMouseMoveEvent);
            timeShaft.RegisterCallback<MouseUpEvent>(TimeShaftMouseUpEvent);
            timeShaft.RegisterCallback<MouseOutEvent>(TimeShaftMouseOutEvent);

            contentContainer.RegisterCallback<MouseMoveEvent>(TimeShaftMouseMoveEvent);
            contentContainer.RegisterCallback<MouseUpEvent>(TimeShaftMouseUpEvent);
            contentContainer.RegisterCallback<MouseOutEvent>(TimeShaftMouseOutEvent);

            selectLine = root.Q<IMGUIContainer>("SelectLine");
            selectLine.onGUIHandler = DrawSelectLine;
        }

        /// <summary>
        /// 鼠标位置得到选中帧索引
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private int GetFrameIndexFromMousePosition(float x)
        {
            return GetFrameIndexPos(contentOffsetPos + x);
        }

        /// <summary>
        /// 鼠标位置得到选中帧索引
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public int GetFrameIndexPos(float x)
        {
            return Mathf.RoundToInt(x / skillEditorConfig.frameUniWidth);
        }

        /// <summary>
        /// 更新时间轴显示
        /// </summary>
        private void UpdateTimeShaftView()
        {
            timeShaft.MarkDirtyLayout();
            selectLine.MarkDirtyLayout();
        }

        /// <summary>
        /// 绘制选中线
        /// </summary>
        private void DrawSelectLine()
        {
            //判断选中帧是否在范围类
            if (currentSelectFramePos >= contentOffsetPos)
            {
                Handles.BeginGUI();
                Handles.color = Color.red;
                float x = currentSelectFramePos - contentOffsetPos;
                Handles.DrawWireCube(new Vector3(x, 10),new Vector3(20,20,20));
                Handles.DrawLine(new Vector3(x, 10), new Vector3(x, timeShaft.contentRect.height + contentViewPort.contentRect.height));
                GUI.color = Color.yellow;
                GUI.Label(new Rect(x - x.ToString().Length * 2.6f, 25, 30, 20), CurrentSelectFrameIndex.ToString());
                Handles.EndGUI();
            }
        }

        /// <summary>
        /// 滚轮事件
        /// </summary>
        /// <param name="evt"></param>
        private void TimeShaftWheel(WheelEvent evt)
        {
            int delta = (int)evt.delta.y;

            if (delta < 0)
            {
                skillEditorConfig.frameUniWidth = Mathf.Clamp(skillEditorConfig.frameUniWidth - delta / 3, 1, 10);
            }
            else
            {
                skillEditorConfig.frameUniWidth = Mathf.Clamp(skillEditorConfig.frameUniWidth - delta / 3, 1, 4);
            }

            //skillEditorConfig.frameUniWidth = Mathf.Clamp(skillEditorConfig.frameUniWidth - delta / 3, 1, SkillEditorConfig.standFrameUniWidth * SkillEditorConfig.maxFrameScaleLV);

            UpdateTimeShaftView();
            UpdateContentSise();
            if (SkillConfig != null)
            {
                ResetView(skillEditorConfig.frameUniWidth);
            }
        }

        /// <summary>
        /// 绘制时间轴
        /// </summary>
        private void DrawTimeShaft()
        {
            //开始绘制
            Handles.BeginGUI();
            Handles.color = Color.white;

            Rect rect = timeShaft.contentRect;//时间轴rect

            //起始帧数
            //98 / 10 = 9.8
            int index = Mathf.CeilToInt(contentOffsetPos / skillEditorConfig.frameUniWidth);

            //起始偏移
            float startOffset = 0f;
            if (index > 0)
            {
                // 98 % 10 = 8
                //10 - 8 = 2
                //向上取整的余数
                startOffset = skillEditorConfig.frameUniWidth - (contentOffsetPos % skillEditorConfig.frameUniWidth);
            }

            //Debug.Log(index + "__" + startOffset);

            //int tickStep = 5;//步长
            int tickStep = SkillEditorConfig.maxFrameScaleLV + 1 - (skillEditorConfig.frameUniWidth / SkillEditorConfig.standFrameUniWidth);
            tickStep = tickStep / 2;
            tickStep = tickStep == 0 ? 1 : tickStep;

            if ((skillEditorConfig.frameUniWidth / SkillEditorConfig.standFrameUniWidth) < 1)
            {
                tickStep = (int)(2.7f + 2.3f * (SkillEditorConfig.standFrameUniWidth / skillEditorConfig.frameUniWidth));
            }

            for (float i = startOffset; i < rect.width; i += skillEditorConfig.frameUniWidth)//i是x轴坐标 单位像素
            {
                //绘制长线和文本
                if (index % tickStep == 0)
                {
                    //Handles.DrawLine 坐标系 锚点左上角 从 p1 到 p2
                    Handles.DrawLine(new Vector3(i, rect.height * (1 - 0.3f)), new Vector3(i, rect.height));
                    string indexLabel = index.ToString();
                    GUI.Label(new Rect(i - indexLabel.Length * 4.5f, rect.y, 30, 20), indexLabel);
                }
                else
                {
                    Handles.DrawLine(new Vector3(i, rect.height * (1 - 0.1f)), new Vector3(i, rect.height));
                }
                index += 1;
            }

            //结束绘制
            Handles.EndGUI();
        }

        #region 时间轴鼠标事件

        private void TimeShaftMouseDownEvent(MouseDownEvent evt)
        {
            timeShaftIsMouseEnter = true;
            IsPlaying = false;
            //获取选中帧索引
            if (GetFrameIndexFromMousePosition(evt.localMousePosition.x) != CurrentSelectFrameIndex)
            {
                CurrentSelectFrameIndex = GetFrameIndexFromMousePosition(evt.localMousePosition.x);
            }
        }

        private void TimeShaftMouseMoveEvent(MouseMoveEvent evt)
        {
            if (timeShaftIsMouseEnter)
            {
                //获取选中帧索引
                if (GetFrameIndexFromMousePosition(evt.localMousePosition.x) != CurrentSelectFrameIndex)
                {
                    CurrentSelectFrameIndex = GetFrameIndexFromMousePosition(evt.localMousePosition.x);
                }
            }
        }

        private void TimeShaftMouseUpEvent(MouseUpEvent evt)
        {
            timeShaftIsMouseEnter = false;
        }

        private void TimeShaftMouseOutEvent(MouseOutEvent evt)
        {
            timeShaftIsMouseEnter = false;
        }

        #endregion 时间轴鼠标事件

        #endregion 时间轴

        #region 控制台

        private ToolbarButton PreviousFrameButton;
        private ToolbarButton PlayButton;
        private ToolbarButton NextFrameButton;
        private ToolbarButton ToTheEndButton;
        private ToolbarButton ToTheHeadButton;
        private IntegerField CurrentFrameTextField;
        private IntegerField FrameCountTextFiled;
        private DropdownField FPSDropDownField;
        private ToolbarButton ClearSceneTopBarButton;

        private FloatField ConsoleAnimationSpeed;
        public float animationSpeed = 1f;

        private void InitConsole()
        {
            PreviousFrameButton = root.Q<ToolbarButton>("PreviousFrameButton");
            PreviousFrameButton.clicked += PreviousFrameButtonClick;

            PlayButton = root.Q<ToolbarButton>("PlayButton");
            PlayButton.clicked += PlayButtonClick;

            NextFrameButton = root.Q<ToolbarButton>("NextFrameButton");
            NextFrameButton.clicked += NextFrameButtonClick;

            CurrentFrameTextField = root.Q<IntegerField>("CurrentFrameTextField");
            CurrentFrameTextField.RegisterValueChangedCallback(CurrentFrameTextFieldValueChange);

            FrameCountTextFiled = root.Q<IntegerField>("FrameCountTextFiled");
            FrameCountTextFiled.RegisterValueChangedCallback(FrameCountTextFiledValueChange);

            ToTheEndButton = root.Q<ToolbarButton>("ToTheEndButton");
            ToTheEndButton.clicked += ToTheEndButtonClick;

            ToTheHeadButton = root.Q<ToolbarButton>("ToTheHeadButton");
            ToTheHeadButton.clicked += ToTheHeadButtonClick;

            ClearSceneTopBarButton = root.Q<ToolbarButton>("ClearSceneTopBarButton");
            ClearSceneTopBarButton.clicked += ClearSceneTopBarButtonClick;

            FPSDropDownField = root.Q<DropdownField>("FPSDropDownField");
            FPSDropDownField.RegisterValueChangedCallback(FPSDropDownFieldValueChange);

            ConsoleAnimationSpeed = root.Q<FloatField>("ConsoleAnimationSpeed");
            ConsoleAnimationSpeed.RegisterValueChangedCallback(ConsoleAnimationSpeedFieldValueChange);
            ConsoleAnimationSpeed.value = animationSpeed;

            if (skillConfig != null)
            {
                SkillConfigObjectField.value = skillConfig;
            }
        }


        //清理场景
        private void ClearSceneTopBarButtonClick()
        {
            foreach (TrackBase item in trackList)
            {
                item.ClearScene();
            }
        }

        private void ConsoleAnimationSpeedFieldValueChange(ChangeEvent<float> evt)
        {
            if (evt.newValue != evt.previousValue)
            {
                animationSpeed = evt.newValue;
            }
        }

        /// <summary>
        /// 改变帧率
        /// </summary>
        /// <param name="evt"></param>
        private void FPSDropDownFieldValueChange(ChangeEvent<string> evt)
        {
            if (evt.previousValue != evt.newValue)
            {
                if (skillConfig != null)
                {
                    skillConfig.FrameRate = int.Parse(evt.newValue);
                }
            }
        }

        /// <summary>
        /// 回到开头
        /// </summary>
        private void ToTheHeadButtonClick()
        {
            CurrentSelectFrameIndex = 0;
        }

        /// <summary>
        /// 会到结尾
        /// </summary>
        private void ToTheEndButtonClick()
        {
            CurrentSelectFrameIndex = CurrentFrameCount;
        }

        /// <summary>
        /// 当前帧文本框变化事件
        /// </summary>
        /// <param name="evt"></param>
        private void CurrentFrameTextFieldValueChange(ChangeEvent<int> evt)
        {
            if (evt.newValue != evt.previousValue)
            {
                if (evt.newValue != CurrentSelectFrameIndex)
                {
                    int num;
                    try
                    {
                        num = evt.newValue;
                    }
                    catch (Exception)
                    {
                        return;
                    }
                    CurrentSelectFrameIndex = num;
                }
            }
        }

        /// <summary>
        /// 帧总数文本框变化事件
        /// </summary>
        /// <param name="evt"></param>
        private void FrameCountTextFiledValueChange(ChangeEvent<int> evt)
        {
            if (evt.newValue != evt.previousValue)
            {
                if (evt.newValue != CurrentFrameCount)
                {
                    int num;
                    try
                    {
                        num = evt.newValue;
                    }
                    catch (Exception)
                    {
                        return;
                    }
                    CurrentFrameCount = num;
                }
            }
        }

        /// <summary>
        /// 下一帧按钮点击
        /// </summary>
        private void NextFrameButtonClick()
        {
            CurrentSelectFrameIndex += 1;
            IsPlaying = false;
        }

        /// <summary>
        /// 播放按钮
        /// </summary>
        private void PlayButtonClick()
        {
            IsPlaying = !IsPlaying;
        }

        /// <summary>
        /// 上一帧按钮点击
        /// </summary>
        private void PreviousFrameButtonClick()
        {
            CurrentSelectFrameIndex -= 1;
            IsPlaying = false;
        }

        #endregion 控制台

        #region 轨道

        private VisualElement ContentListView;//右侧内容
        private VisualElement TrackMenuParent;//轨道菜单
        private ScrollView MainContentView;   //右侧滑动
        private ScrollView TrackMenuScrollView;   //左侧侧滑动

        private List<TrackBase> trackList = new List<TrackBase>();


        /// <summary>
        /// 左侧允许上下滑动,并且同步左右
        /// </summary>
        /// <param name="value"></param>
        private void TrackMenuScrollViewVerticalValueChanged(float value)
        {
            Vector2 pos = MainContentView.scrollOffset;
            pos.y = TrackMenuScrollView.scrollOffset.y;
            MainContentView.scrollOffset = pos;
        }

        /// <summary>
        /// 滚轮右侧左右移动
        /// </summary>
        /// <param name="evt"></param>
        private void MainContentViewScollViewEvent(WheelEvent evt)
        {
            Vector2 pos = MainContentView.scrollOffset;
            pos.y = TrackMenuScrollView.scrollOffset.y;
            pos.x += evt.delta.y * 5f;
            MainContentView.scrollOffset = pos;
        }

        /// <summary>
        /// 更新容器体积
        /// </summary>
        private void UpdateContentSise()
        {
            if (ContentListView != null)
                ContentListView.style.width = skillEditorConfig.frameUniWidth * CurrentFrameCount;
        }


        /// <summary>
        /// 初始化主要容器
        /// </summary>
        private void InitContent()
        {
            ContentListView = root.Q<VisualElement>("ContentListView");
            TrackMenuParent = root.Q<VisualElement>("TrackMenuList");
            MainContentView = root.Q<ScrollView>("MainContentView");
            TrackMenuScrollView = root.Q<ScrollView>("TrackMenuScrollView");
            MainContentView = root.Q<ScrollView>("MainContentView");
            TrackMenuScrollView.verticalScroller.valueChanged += TrackMenuScrollViewVerticalValueChanged;
            MainContentView.RegisterCallback<WheelEvent>(MainContentViewScollViewEvent);

            UpdateContentSise();
            InitTrack();
        }

        /// <summary>
        /// 初始化轨道
        /// </summary>
        private void InitTrack()
        {
            if (SkillConfig != null)
            {
                DestroyTracksView();

                foreach (KeyValuePair<string, SkillTrackDataBase> item in SkillConfig.trackDataDic)
                {
                    TrackBase track = Activator.CreateInstance(Type.GetType(item.Key)) as TrackBase;
                    string trackName = Type.GetType(item.Key).GetField("TrackName", BindingFlags.Static | BindingFlags.Public).GetValue(track).ToString();
                    if (track != null)
                    {
                        track.Init(TrackMenuParent, ContentListView, skillEditorConfig.frameUniWidth, item.Value, trackName);
                        trackList.Add(track);
                    }
                }
            }
            else
            {
                DestroyTracksView();
            }
        }

        /// <summary>
        /// 公开的InitTrack
        /// </summary>
        public void P_InitTrack()
        { InitTrack(); }

        #endregion 轨道

        #region 预览

        private bool isPlaying;

        public bool IsPlaying
        {
            get { return isPlaying; }
            set
            {
                isPlaying = value;
                if (isPlaying)
                {
                    startTime = DateTime.Now;
                    startFrameIndex = CurrentSelectFrameIndex;
                    for (int i = 0; i < trackList.Count; i++)
                    {
                        trackList[i].OnPlay(currentFrameCount);
                    }
                    EditorCoroutineUtility.StartCoroutine(SkillUpdate(), this);
                }
                else
                {
                    for (int i = 0; i < trackList.Count; i++)
                    {
                        trackList[i].OnStop();
                    }
                }
            }
        }

        private DateTime startTime;
        private int startFrameIndex;

        private IEnumerator SkillUpdate()
        {
            while (IsPlaying)
            {
                //时间差
                float time = (float)DateTime.Now.Subtract(startTime).TotalSeconds;
                //帧率
                float frameRate;
                if (skillConfig != null) { frameRate = skillConfig.FrameRate; }
                else { frameRate = skillEditorConfig.defaltFrameRote; }
                //计算当前帧
                CurrentSelectFrameIndex = (int)(time * frameRate * animationSpeed) + startFrameIndex;
                if (CurrentSelectFrameIndex == CurrentFrameCount)
                {
                    IsPlaying = false;
                }
                yield return null;
            }
            yield break;
        }

        /// <summary>
        /// 驱动技能表现
        /// </summary>
        private void TickSkill()
        {
            //驱动技能表现
            if (skillConfig != null && currentPreviewGameObject != null)
            {
                //驱动动画
                for (int i = 0; i < trackList.Count; i++)
                {
                    trackList[i].TickView(CurrentSelectFrameIndex);
                }
            }
        }

        #endregion 预览

        /// <summary>
        /// 重置轨道数据
        /// </summary>
        private void ResetTrackData()
        {
            foreach (TrackBase item in trackList)
            {
                item.OnConfigChange();
            }
        }

        /// <summary>
        /// 保存Config配置刷新数据
        /// </summary>
        public void AutoSaveConfig()
        {
            if (skillEditorConfig.isAutoSaveConfig)
            {
                SaveConfig();
            }
        }

        /// <summary>
        /// 直接保存
        /// </summary>
        public void SaveConfig()
        {
            if (skillConfig != null)
            {
                EditorUtility.SetDirty(skillConfig);
                AssetDatabase.SaveAssetIfDirty(skillConfig);
                ResetTrackData();
            }
        }

        /// <summary>
        /// 删除所有轨道视图
        /// </summary>
        public void DestroyTracksView()
        {
            //删除所有轨道
            foreach (var item in trackList)
            {
                item.DestroyTrackView();
            }
            trackList.Clear();
        }

        public void ResetView(float frameWidth)
        {
            foreach (var item in trackList)
            {
                item.ResetView(frameWidth);
            }
        }

        public void StartCoroutin(IEnumerator objects)
        {
            EditorCoroutineUtility.StartCoroutine(objects, this);
        }

        public void UnSelectAll()
        {
            foreach (TrackBase item in trackList)
            {
                item.UnSelectAll();
            }
        }

        private void OnSelectionChange()
        {
            SkillConfig skillConfig = Selection.activeObject as SkillConfig;
            if (skillConfig != null)
                SkillConfigObjectField.value = skillConfig;
        }
    }
}
