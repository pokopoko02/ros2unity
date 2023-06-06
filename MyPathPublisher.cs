using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RosSharp.RosBridgeClient.MessageTypes.Nav;

namespace RosSharp.RosBridgeClient
{
        public class MyPathPublisher : UnityPublisher<MessageTypes.Nav.Path>
    {   
        Path path = new Path();
        public Transform PublishedTransform;
        public string FrameId = "map";//"Unity";

        [SerializeField] Transform target;     // 目標地点
        [SerializeField] UnityEngine.AI.NavMeshAgent agent;
        private UnityEngine.AI.NavMeshPath Unitypath;

        private MessageTypes.Geometry.PoseStamped message;//MessageTypes.Nav.Path message;

        protected override void Start()
        {
            base.Start();
            // NavMeshAgent に目的地を設定する
            agent.SetDestination (target.position);

            // 経路取得用のインスタンス作成
            Unitypath = new UnityEngine.AI.NavMeshPath ();
            // 明示的な経路計算実行
            agent.CalculatePath (target.position, Unitypath);
//            foreach (Vector3 i in Unitypath.corners)print(i);//Transform.Position描写箇所

            InitializeMessage();
        }
        private void FixedUpdate()
        {
            UpdateMessage();
            // NavMeshAgent に目的地を設定する
            agent.SetDestination (target.position);
            // 経路取得用のインスタンス作成
            Unitypath = new UnityEngine.AI.NavMeshPath ();
            // 明示的な経路計算実行
            agent.CalculatePath (target.position, Unitypath);
            //if(i%10==0)
//            foreach (Vector3 i in path.corners)print(i);//Transform.Position描写箇所
        }

        private void InitializeMessage()
        {
            message = new MessageTypes.Geometry.PoseStamped
            {
                header = new MessageTypes.Std.Header()
                {
                    frame_id = FrameId
                }
            };
        }

        private void UpdateMessage()
        {
            message.header.Update();
            List<MessageTypes.Geometry.PoseStamped> posesList = new List<MessageTypes.Geometry.PoseStamped>();
            foreach (Vector3 i in Unitypath.corners)
            {
                MessageTypes.Geometry.PoseStamped message = new MessageTypes.Geometry.PoseStamped();
                message.header = path.header;
                message.pose.position = GetGeometryPoint(i.Unity2Ros());

                posesList.Add(message);
            }
            
            System.DateTime now = System.DateTime.Now;
            // Set path.header
            path.header = new MessageTypes.Std.Header()
            {
                frame_id = FrameId,
                stamp = new MessageTypes.Std.Time
                {
                    secs = (uint)now.Second,
                    nsecs = (uint)now.Millisecond * 1000 // Convert milliseconds to nanoseconds
                }
            //    stamp = RosSharp.RosBridgeClient.MessageTypes.Std.Time.Now()
            };

            path.poses = posesList.ToArray();
            print(path);
            Publish(path);
        }/*
            // これだと無理 message.poses[0] = Vector3(0,0,0);
    //        GetGeometryPoint(PublishedTransform.position);
//原本            message.poses[i] = GetGeometryPoint(PublishedTransform.position);
            //message.pose.orientation = GetGeometryQuaternion(PublishedTransform.rotation.Unity2Ros());

            foreach (Vector3 i in Unitypath.corners){
                message.pose.position = GetGeometryPoint(PublishedTransform.position.Unity2Ros());
                Publish(message);
            } //Transform.Position描写箇所
            print(Unitypath.corners.Length);
            //path.poses = message.pose;

        }*/

        private MessageTypes.Geometry.Point GetGeometryPoint(Vector3 position)
        {
            MessageTypes.Geometry.Point geometryPoint = new MessageTypes.Geometry.Point();
            geometryPoint.x = position.x;//z;
            geometryPoint.y = position.y;//x*(-1);
            geometryPoint.z = position.z;//y;

            return geometryPoint;
        }
/*
        private MessageTypes.Nav.Path GetGeometryPoint(Vector3 position)
        {
            MessageTypes.Nav.Path NavPoint = new MessageTypes.Nav.Path();
            NavPoint.poses.pose.position.x = position.x;//z;
            NavPoint.poses.pose.position.y = position.y;//x*(-1);
            NavPoint.poses.pose.position.z = position.z;//y;

            return NavPoint;
        }
        */
/*
         private MessageTypes.Geometry.Point GetGeometryPoint(Vector3 position)
        {
            MessageTypes.Geometry.Point geometryPoint = new MessageTypes.Geometry.Point();
            geometryPoint.x = position.x;//z;
            geometryPoint.y = position.y;//x*(-1);
            geometryPoint.z = position.z;//y;

            return geometryPoint;
        }
        */

       /* private MessageTypes.Geometry.Quaternion GetGeometryQuaternion(Quaternion quaternion)
        {
            MessageTypes.Geometry.Quaternion geometryQuaternion = new MessageTypes.Geometry.Quaternion();
            geometryQuaternion.x = quaternion.x;//z;
            geometryQuaternion.y = quaternion.y;//x*(-1);
            geometryQuaternion.z = quaternion.z;//y;
            geometryQuaternion.w = quaternion.w;//*(-1);
            return geometryQuaternion;
        }*/

    }
}
