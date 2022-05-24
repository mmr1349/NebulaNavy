
for (i=[0:1]){
	translate([wing_x_offset(),yoffo[i],0])rotate([rot_mat[i],0,0])union(){
		// upper leading //
		translate([(lower_scale()-upper_leading_scale())*wing_upper_axis(),0]){
			scale([upper_leading_scale(),1]){
				difference(){

					circle(r=wing_upper_axis(),$fn=N());
					translate([0,pos_neg()[i]*wing_upper_axis()/2])square([wing_upper_axis()*2,wing_upper_axis()],center=true);
					translate([-wing_upper_axis()/2,0])square([wing_upper_axis(),wing_upper_axis()*2],center=true);
				}
			}
		}
		// lower //
		scale([lower_scale()*(wing_upper_axis()/wing_lower_axis()),1]){
			difference(){
				circle(r=wing_lower_axis(),$fn=N());
				translate([0,-1*pos_neg()[i]*wing_lower_axis()/2])square([wing_lower_axis()*2,wing_lower_axis()],center=true);
			}
		}
		// upper trailing //
		translate([(lower_scale()-upper_leading_scale())*wing_upper_axis(),0]){
			scale([upper_trailing_scale(),1]){
				difference(){
					circle(r=wing_upper_axis(),$fn=N());
					translate([0,pos_neg()[i]*wing_upper_axis()/2])square([wing_upper_axis()*2,wing_upper_axis()],center=true);
					translate([wing_upper_axis()/2,0])square([wing_upper_axis(),wing_upper_axis()*2],center=true);
				}
			}
		}
	}
}
