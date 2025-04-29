using UnityEngine;

public class SoftBodyVertex : MonoBehaviour
{
        private int ID;
        private Vector3 position;
        private Vector3 velocity;
        private Vector3 force;
    
    
        public SoftBodyVertex(int id, Vector3 position)
        {
            ID = id;
            this.position = position;
        }
        
       
        
        public void ShakeMesh(Vector3 target, float mass, float stiffness, float damping)
        {
            force = (target - position) * stiffness;
            velocity = (velocity + force / mass) * damping;
            position += velocity;
    
            if ((velocity + force + force / mass).magnitude < 0.001f)
            {
                position = target;
            }
        }
        
        //Getters and setters:\
    
        public int GetId()
        {
            return ID;
        }
    
        public Vector3 GetPosition()
        {
            return position;
        }
}
