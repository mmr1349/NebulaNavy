//// VARIABLES ////

use <spaceship.scad>

// BARREL //
function outer_barrel_radius() = 0.6;
function inner_barrel_radius() = 0.85*outer_barrel_radius();
function barrel_length() = 20;
barrel_x_diff=[0,3];

// MUZZLE CONE //
function cone_length() = 5;
function cone_base_radius() = 1;
function cone_angle() = atan(cone_length()/cone_base_radius());
function cone_intersection_distance() = inner_barrel_radius() * tan(cone_angle()) - 0.04;

//// GENERATION CODE ////

for (i = [1:2]){
translate([barrel_x_diff[i],0,0])
	difference(){
		union(){
			translate([0,0,cone_intersection_distance()]) cylinder(h=barrel_length(),r=outer_barrel_radius(),$fn=N());
			rotate_extrude($fn=N())
				polygon(points=[[0,0],[cone_base_radius(),0],[0,cone_length()]]);

		}
		union(){
			translate([0,0,cone_intersection_distance()]) cylinder(h=barrel_length(),r=inner_barrel_radius(),$fn=N());
			scale(0.9)
				rotate_extrude($fn=N())
				polygon(points=[[0,0],[cone_base_radius(),0],[0,cone_length()]]);

		}
	}
}
