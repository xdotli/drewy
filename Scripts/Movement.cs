using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 5f; // Speed of the drone
    public float radius = 5f; // Radius of the circular path


    public Vector3 pos;
    public Vector3 vel = new Vector3(0f, 0f, 0f);
    public float yaw_angle = 0f;
    // Hard-coded desired position for now. 
    public Vector3 desired_pos;
    public float desired_yaw_angle;
    // control variables
    public float ctrl_roll = 0f;
    public float ctrl_pitch = 0f;
    public float ctrl_yaw_dot = 0f;
    public float ctrl_thrust = 0f;
    private float gravity = 9.81f;

    // control gain
    // the last column is not used.
    public Matrix4x4 K_pos;
    public Matrix4x4 K_others;
    public Vector4 x_rel;
    public Vector4 dx_rel;
    public Vector4 x_rel_trans;
    public Vector4 dx_rel_trans;

    void Start()
    {
        pos = transform.position;
        // desired_pos = new Vector3(0f, 58.5f, 5f);
        desired_yaw_angle  = Mathf.Deg2Rad * 45f;
        // Correctly initialize K_pos
        K_pos = new Matrix4x4();
        K_pos.SetRow(0, new Vector4(0f, 10f, 0f, 0f));
        K_pos.SetRow(1, new Vector4(-10f, 0f, 0f, 0f));
        K_pos.SetRow(2, new Vector4(0f, 0f, 0f, 0f));
        K_pos.SetRow(3, new Vector4(0f, 0f, -200f, 0f));

        // Correctly initialize K_others
        K_others = new Matrix4x4();
        K_others.SetRow(0, new Vector4(0f, 1.74f, 0f, 0f));
        K_others.SetRow(1, new Vector4(-1.74f, 0f, 0f, 0f));
        // K_others.SetRow(0, new Vector4(0f, 1.74f, 0f, 0f));
        // K_others.SetRow(1, new Vector4(-1.74f, -0f, 0f, 0f));
        K_others.SetRow(2, new Vector4(-0f, 0f, 0f, -100f));
        K_others.SetRow(3, new Vector4(-0f, -0f, -28.28f, 0f));
    }

    void Update()
    {   

        UpdatePositionTrackingControl();
        x_rel = new Vector4(pos.x - desired_pos.x, pos.y - desired_pos.y, pos.z - desired_pos.z, 0);
        Vector3 pos_dot = new Vector3(- 10 * x_rel.x, - 10 * x_rel.y, - 10 * x_rel.z);
        pos_dot.x = Mathf.Clamp(pos_dot.x, -10, 10);
        pos_dot.y = Mathf.Clamp(pos_dot.y, -10, 10);
        pos_dot.z = Mathf.Clamp(pos_dot.z, -10, 10);
        // Debug.Log(pos_dot);
        
        // Vector3 vel_dot = new Vector3(ctrl_thrust * (Mathf.Sin(ctrl_pitch) * Mathf.Cos(yaw_angle) - Mathf.Sin(ctrl_roll) * Mathf.Sin(yaw_angle)),
        //                                 ctrl_thrust * (Mathf.Sin(ctrl_pitch) * Mathf.Sin(yaw_angle) + Mathf.Sin(ctrl_roll) * Mathf.Cos(yaw_angle)),
        //                                 ctrl_thrust * (Mathf.Cos(ctrl_pitch) * Mathf.Cos(ctrl_roll)) - gravity);
        // Vector3 vel_dot = new Vector3(gravity * Mathf.Tan(ctrl_pitch),
        //                                 -gravity * Mathf.Tan(ctrl_roll),
        //                                 ctrl_thrust - gravity);                                        
        // Debug.Log(vel_dot);
        // // Update states
        pos += pos_dot * Time.deltaTime;
        // vel += vel_dot * Time.deltaTime;
        yaw_angle += ctrl_yaw_dot * Time.deltaTime;
        
        // Set rendered image to new state
        transform.position = pos;

        // FIX THIS PART
        transform.rotation = Quaternion.Euler(ctrl_roll * Mathf.Rad2Deg, yaw_angle * Mathf.Rad2Deg, ctrl_pitch * Mathf.Rad2Deg);
    }

    void UpdatePositionTrackingControl()
    {
        x_rel = new Vector4(pos.x - desired_pos.x, pos.y - desired_pos.y, pos.z - desired_pos.z, 0);
        float angle_diff = yaw_angle - desired_yaw_angle;
        float rs = Mod(angle_diff + Mathf.PI, 2 * Mathf.PI);
        angle_diff = rs - Mathf.PI;
        dx_rel = new Vector4(vel.x, vel.y, vel.z, yaw_angle - desired_yaw_angle);
        
        x_rel_trans = RotateCoordinates(x_rel, yaw_angle);
        
        dx_rel_trans  = RotateCoordinates(dx_rel, yaw_angle);

        // Compute state feedback control
        Vector4 u_ref = new Vector4(0, 0, 0, gravity);
        Vector4 u = u_ref + K_pos * x_rel_trans + K_others * dx_rel_trans; // This requires implementing matrix-vector multiplication
        // Debug.Log("---");
        // Debug.Log(K_pos * x_rel_trans);
        // Debug.Log(K_others * dx_rel_trans);
        // Set control variables
        ctrl_roll = Mathf.Clamp(u.x, -Mathf.Deg2Rad * 10, Mathf.Deg2Rad * 10);
        ctrl_pitch = Mathf.Clamp(u.y, -Mathf.Deg2Rad * 10, Mathf.Deg2Rad * 10);
        ctrl_yaw_dot = Mathf.Clamp(u.z, -Mathf.Deg2Rad * 10, Mathf.Deg2Rad * 10);
        ctrl_thrust = Mathf.Clamp(u.w, 6, 14);
        // ctrl_roll = 0;
        // ctrl_pitch = 0;
        // ctrl_yaw_dot = 0;
        // ctrl_thrust = 11;
    }

    Vector4 RotateCoordinates(Vector4 coordinates, float angle)
    {
        float cos_yaw = Mathf.Cos(angle);
        float sin_yaw = Mathf.Sin(angle);

        float rot_x = cos_yaw * coordinates.x + sin_yaw * coordinates.y;
        float rot_y = -sin_yaw * coordinates.x + cos_yaw * coordinates.y;

        return new Vector4(rot_x, rot_y, coordinates.z, coordinates.w);
    }
    private float Mod(float a, float b)
    {
        return a - b * Mathf.Floor(a / b);
    }    
}
