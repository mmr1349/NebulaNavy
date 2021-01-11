use <spaceship.scad>

//i=1;

rot_mat=[90,270];
//for (i=[0:1]){
	difference(){
		translate([wing_x_offset(),0,0])rotate([rot_mat[i],0,0])cylinder(r=upper_leading_radius(), h=wing_length(), $fn=N()*.3);
		translate([wing_x_offset(),upper_leading_rear_cutout_offset()[i],-upper_leading_radius()/2])cube([upper_leading_radius()*2,wing_length(),upper_leading_radius()],center=true);
	}
	translate([wing_x_offset()-(lower_scale()*upper_leading_radius())/2,0,0])rotate([rot_mat[i],0,0])linear_extrude(height=wing_length())scale([2,1,1])circle(upper_leading_radius(),$fn=N()*.3);
//}
