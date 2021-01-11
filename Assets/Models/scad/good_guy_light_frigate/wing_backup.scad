rot_mat=[270,90];
for (i=[0:1]){

	// upper leading //
	difference(){
		translate([wing_x_offset()-(upper_leading_radius()*(upper_leading_scale()-1)),0,0])scale([upper_leading_scale(),1,1])rotate([rot_mat[i],0,0])cylinder(r=upper_leading_radius(), h=wing_length(), $fn=N()*.3);
		translate([wing_x_offset()-(upper_leading_radius()*(upper_leading_scale()-1)),upper_leading_rear_cutout_offset()[i],-upper_leading_radius()/2])cube([upper_leading_radius()*2*upper_leading_scale(),wing_length()+1,upper_leading_radius()],center=true);
		translate([wing_x_offset()-(upper_leading_radius()*(upper_leading_scale()+.5)),upper_leading_rear_cutout_offset()[i],0])cube([upper_leading_radius()*upper_leading_scale(),wing_length()+1,upper_leading_radius()*2],center=true);
	}
	// lower //
	difference(){
		translate([wing_x_offset()-(upper_leading_radius()*(lower_scale()-1)),0,0])rotate([rot_mat[i],0,0])scale([lower_scale(),1,1])cylinder(r=upper_leading_radius(),h=wing_length(),$fn=N()*.3);
		translate([wing_x_offset()-(upper_leading_radius()*(lower_scale()-1)),upper_leading_rear_cutout_offset()[i],+upper_leading_radius()/2])cube([upper_leading_radius()*2*lower_scale(),wing_length()+1,upper_leading_radius()],center=true);
	}

}
