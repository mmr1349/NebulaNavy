module airfoil(wing_upper_axis=0.3, wing_lower_axis=0.2, upper_leading_scale=2, lower_scale=4, N=50) {

	upper_leading_z_offset=[-wing_upper_axis,wing_upper_axis];
	//upper_leading_rear_cutout_offset=[wing_length/2,(-wing_length)/2];
	upper_trailing_scale = lower_scale*2-upper_leading_scale;

	union(){
		// upper leading //
		translate([(lower_scale-upper_leading_scale)*wing_upper_axis,0]){
			scale([upper_leading_scale,1]){
				difference(){

					circle(r=wing_upper_axis,$fn=N);
					translate([0,-1*wing_upper_axis/2])square([wing_upper_axis*2,wing_upper_axis],center=true);
					translate([-wing_upper_axis/2,0])square([wing_upper_axis,wing_upper_axis*2],center=true);
				}
			}
		}
		// lower //
		scale([lower_scale*(wing_upper_axis/wing_lower_axis),1]){
			difference(){
				circle(r=wing_lower_axis,$fn=N);
				translate([0,wing_lower_axis/2])square([wing_lower_axis*2,wing_lower_axis],center=true);
			}
		}
		// upper trailing //
		translate([(lower_scale-upper_leading_scale)*wing_upper_axis,0]){
			scale([upper_trailing_scale,1]){
				difference(){
					circle(r=wing_upper_axis,$fn=N);
					translate([0,-1*wing_upper_axis/2])square([wing_upper_axis*2,wing_upper_axis],center=true);
					translate([wing_upper_axis/2,0])square([wing_upper_axis,wing_upper_axis*2],center=true);
				}
			}
		}
	}
}
airfoil();
