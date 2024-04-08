混合动画技能编辑器和场景动画组件
---
* 描述：基于Timeline，Animancer，Playable的技能编辑器，支持动画混合。场景动画组件，支持外设输入到人物技能行为一体化控制。
* 流程思路：通过InputComponent划分输入枚举，StateComponent内部为FSM状态机，接受输入划分State，State序列化为ScriptableObject存储，同时基于此数据信息可由Editor模式下创建TimelineAsset，并设置轨道Track以及Playable，PlayableBehaviour，在PlayableBehaviour借助Animancer实现动画混合。
* 过程中的Bug：TimelineAsset的创建与Track的创建不能嵌套执行。
* 待实现：TimelineEditor的编写，State的分层管理Tag系统。
* 参考链接：zhuanlan.zhihu.com/p/513872343
            zhuanlan.zhihu.com/p/380124248
            zhuanlan.zhihu.com/p/380710676
            zhuanlan.zhihu.com/p/371165024
