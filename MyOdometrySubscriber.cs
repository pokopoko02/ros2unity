
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class MyOdometrySubscriber : UnitySubscriber<MessageTypes.Geometry.PoseWithCovarianceStamped>

    {
        public Transform PublishedTransform;

        private Vector3 position;
        private Quaternion rotation;
        private Vector3 rota;
        Vector3 vec3;//         = new Vector3(0.0f,0.0f,-90f);
        Quaternion qua;
        private bool isMessageReceived;

        protected override void Start()
		{
            vec3= new Vector3(0.0f,0.0f,-90f);
            qua = Quaternion.Euler(vec3);
			base.Start();
		}
		
        private void Update()
        {
            if (isMessageReceived)
                ProcessMessage();
            
        }
        
        protected override void ReceiveMessage(MessageTypes.Geometry.PoseWithCovarianceStamped message)
        {
            position = GetPosition(message).Ros2Unity(); 
            isMessageReceived = true;
            rotation = GetRotation(message).Ros2Unity();
            float x=rotation.eulerAngles.x;
            float y=0;
            float z=-90;
            //rotation.eulerAngles=new Vector3(x,y,z);
            rotation=Quaternion.Euler(x,y,z);
//            print(rotation);
//            print(rotation.eulerAngles);
            print("aaaaa"+rotation.eulerAngles.x);
            //print("bbbbb"+rotation.x+" "+rotation.y+" "+rotation.z+" "+rotation.w);
        }
        private void ProcessMessage()
        {
            PublishedTransform.position = position;
            //rotation=rotation*qua;
            PublishedTransform.rotation = rotation;
            //this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, x);
        }

        private Vector3 GetPosition(MessageTypes.Geometry.PoseWithCovarianceStamped message)
        {
            print("aaaaaaaaaaaaaaaaaaa");
            return new Vector3(
                (float)message.pose.pose.position.y*(-1),
                (float)message.pose.pose.position.z,
                (float)message.pose.pose.position.x);
        }
        
        private Quaternion GetRotation(MessageTypes.Geometry.PoseWithCovarianceStamped message)
        {
            print("#####"+(float)(message.pose.pose.orientation.y*(-1)));
            return new Quaternion(
                (float)(message.pose.pose.orientation.y*(-1)),
                (float)(message.pose.pose.orientation.z),
                (float)(message.pose.pose.orientation.x),
                (float)(message.pose.pose.orientation.w*(-1)));
        }

        /*
        private Quaternion GetRotation(MessageTypes.Geometry.PoseWithCovarianceStamped message)
        {
            rotation = new Quaternion(
                (float)(message.pose.pose.orientation.y*(-1)),
                (float)(message.pose.pose.orientation.z),
                (float)(message.pose.pose.orientation.x),
                (float)(message.pose.pose.orientation.w*(-1)));
            rota=Quaternion.Euler(rotation);
            rota=new Vector3((float)rota.x,(float)0.0f,(float)-90.0f);

        }*/
        /*
        private Quaternion GetRotation(MessageTypes.Geometry.PoseWithCovarianceStamped message)
        {
            return new Quaternion(
                (float)(message.pose.pose.orientation.y*(-1)),
                (float)(message.pose.pose.orientation.z),
                (float)(message.pose.pose.orientation.x),
                (float)(message.pose.pose.orientation.w*(-1)));
        }*/
        /*
        
        private Quaternion GetRotation(MessageTypes.Geometry.PoseWithCovarianceStamped message)
        {
            return new Quaternion(
                (float)(message.pose.pose.orientation.y*(-1)+0.4469983318f*(-1)),
                (float)(message.pose.pose.orientation.z+0.7240368081f),
                (float)(message.pose.pose.orientation.x+0.4469983318f),
                (float)(message.pose.pose.orientation.w*(-1))+0.2759631919f*(-1));
        }
        */
    }
}
