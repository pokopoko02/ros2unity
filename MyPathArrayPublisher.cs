/*
© Siemens AG, 2017-2018
Author: Dr. Martin Bischoff (martin.bischoff@siemens.com)

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at
<http://www.apache.org/licenses/LICENSE-2.0>.
Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace RosSharp.RosBridgeClient
{
        public class MyPathArrayPublisher : UnityPublisher<MessageTypes.Geometry.PoseStamped>//public class MyPoseArrayPublisher : UnityPublisher<MessageTypes.Nav.Path>//Geometry.PoseStamped>
    {   
//        [SerializeField] Transform start, goal;
        public Transform PublishedTransform;
        public string FrameId = "odom";//"Unity";

        [SerializeField] Transform target;     // 目標地点
        [SerializeField] UnityEngine.AI.NavMeshAgent agent;
        private UnityEngine.AI.NavMeshPath path;



        private MessageTypes.Geometry.PoseStamped message;//MessageTypes.Nav.Path message;

        protected override void Start()
        {
            base.Start();
            // NavMeshAgent に目的地を設定する
            agent.SetDestination (target.position);

            // 経路取得用のインスタンス作成
            path = new UnityEngine.AI.NavMeshPath ();
            // 明示的な経路計算実行
            agent.CalculatePath (target.position, path);
       //     print("はじめ");
            
 //           var prismaticJointBody = desired.GetComponent<ArticulationBody>();
   //         message.position[i+numJoints] = prismaticJointBody.jointPosition[0]; 

//            var PubPos=desired.GetComponent<>();
  //          message.poses[i] = GetGeometryPoint(PublishedTransform.position);

//            print(path.coners);
            foreach (Vector3 i in path.corners)print(i);//Transform.Position描写箇所

            InitializeMessage();
        }
/*        private void Update()
        {
            print(path);
            print("my_pose");
        }*/
        private void FixedUpdate()
        {
            UpdateMessage();
            //print("my_pose");

            // NavMeshAgent に目的地を設定する
            agent.SetDestination (target.position);

            // 経路取得用のインスタンス作成
            path = new UnityEngine.AI.NavMeshPath ();

            // 明示的な経路計算実行
            agent.CalculatePath (target.position, path);

        

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
            // これだと無理 message.poses[0] = Vector3(0,0,0);
    //        GetGeometryPoint(PublishedTransform.position);
//原本            message.poses[i] = GetGeometryPoint(PublishedTransform.position);
            //message.pose.orientation = GetGeometryQuaternion(PublishedTransform.rotation.Unity2Ros());

            foreach (Vector3 i in path.corners){
                message.pose.position = GetGeometryPoint(PublishedTransform.position.Unity2Ros());
                Publish(message);           
                print(message);

            } //Transform.Position描写箇所
        }
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