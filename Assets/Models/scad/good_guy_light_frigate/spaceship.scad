//// MODULES ////

use <airfoil.scad>
use <ship_hull.scad>
use <forward_landing_gear.scad>

//// VARIABLES ////
function N() = 30;
function pos_neg() = [1,-1];

// HULL //
hull_length = 10;
hull_radius = 1;

// BRIDGE //
bridge_length = 3;
bridge_radius = 0.7;
bridge_forward = 4;
bridge_vertical = .5;

bridge_interior_scale = 0.95;

// ENGINEERING BRIDGE //
engineering_bridge_length = 2;
engineering_bridge_radius = 0.7;
engineering_bridge_forward = -4;
engineering_bridge_vertical = .6;

// PRIMARY ENGINE //
primary_engine_length=3;
primary_engine_radius=hull_radius+0;
primary_engine_x_offset=-8.5;
primary_engine_cone_x_offset=-2.3;

primary_engine_cutout_width=.18;
primary_engine_cutout_depth=.2;

// SECONDARY ENGINE //
secondary_engine_length = 4;
secondary_engine_radius = 0.4;
secondary_engine_y_offset = hull_radius+secondary_engine_radius+0.4;
secondary_engine_x_offset =-8.5;
num_secondary_engines = 6;
secondary_engine_buffer = 0.3+secondary_engine_radius*2;
sec_y = [secondary_engine_y_offset,-secondary_engine_y_offset,secondary_engine_y_offset,-secondary_engine_y_offset,secondary_engine_y_offset,-secondary_engine_y_offset];
sec_z = [0,0,secondary_engine_buffer,secondary_engine_buffer,-secondary_engine_buffer,-secondary_engine_buffer];
secondary_engine_cone_x_offset=-2.7;

// SECONDARY ENGINE TRUSSES //
truss_radius=0.15;
truss_angle_adjust=[90,-90,90,-90];
truss_x_offset=[-7.5,-7.5,-9.5,-9.5];
truss_y_offset=[secondary_engine_y_offset,-secondary_engine_y_offset,secondary_engine_y_offset,-secondary_engine_y_offset];
v_truss_length = 2*secondary_engine_buffer;

// REAR LANDING GEAR //
housing_x_offset = (truss_x_offset[3]-truss_x_offset[1])/2 + (truss_x_offset[1]);
housing_y_offset = [1.2,-1.2];
housing_x = -(truss_x_offset[3]-truss_x_offset[1])+0.6;
housing_y = 0.3;

// CARGO BAY //
function cargo_bay_length() = 7;
function cargo_bay_y_radius() = 0.8;
function cargo_bay_z_radius() = 0.5;
function cargo_bay_y_offset() = 0.5;
function cargo_bay_z_offset() = -0.5;

// FORWARD WINGS //

function forward_wing_x_offset() = 6;

function forward_wing_length() = 2;

function forward_wing_upper_axis()=0.3;
function forward_wing_lower_axis()=0.2;

function forward_wing_tip_scale()=.3;

// upper leading //
function forward_upper_leading_scale() = 2;
//function upper_leading_z_offset()=[-wing_vertical_radius(),wing_vertical_radius()];
//function upper_leading_rear_cutout_offset()=[forward_wing_length()/2,(-forward_wing_length())/2];

// lower //
function forward_lower_scale() = 4;

// upper trailing //
function forward_upper_trailing_scale() = forward_lower_scale()*2-forward_upper_leading_scale();

// REAR WINGS //

function rear_wing_x_offset() = -3;

function rear_wing_length() = 7;

function rear_wing_upper_axis()=0.3;
function rear_wing_lower_axis()=0.2;

// upper leading //
function rear_upper_leading_scale() = 3;
//function upper_leading_z_offset()=[-wing_vertical_radius(),wing_vertical_radius()];
//function upper_leading_rear_cutout_offset()=[rear_wing_length()/2,(-rear_wing_length())/2];

// lower //
function rear_lower_scale() = 9;

// upper trailing //
function rear_upper_trailing_scale() = rear_lower_scale()*2-rear_upper_leading_scale();

// DOOR //

//// GENERATION CODE ////

//difference(){
union(){
	// HULL //
	ship_hull(hull_length,hull_radius,N());

	// BRIDGE //
	translate([bridge_forward,0,bridge_vertical]) scale([bridge_length,bridge_radius,bridge_radius]) sphere (r=1.0,$fn=N());

	// ENGINEERING BRIDGE //
	translate([engineering_bridge_forward,0,engineering_bridge_vertical]) scale([engineering_bridge_length,engineering_bridge_radius,engineering_bridge_radius]) sphere (r=1.0,$fn=N());

	// PRIMARY ENGINE //
	difference() {
		translate([primary_engine_x_offset,0,0]) rotate([0,90,0])
			union() {
				cylinder(h=primary_engine_length, r=primary_engine_radius, center=true, $fn=N());
				difference() {
					translate([0,0,primary_engine_cone_x_offset]) scale([primary_engine_radius,primary_engine_radius,1.5]) sphere(r=1.0,$fn=N());
					translate([0,0,primary_engine_cone_x_offset-1]) cylinder (h=2,r=primary_engine_radius+.05,center=true,$fn=N());
					translate([0,0,primary_engine_cone_x_offset]) scale([primary_engine_radius,primary_engine_radius,1.5]) sphere(r=0.9,$fn=N());
				}
			}
		translate([primary_engine_x_offset+primary_engine_length/2,0,0])
			rotate([0,90,0])
			rotate_extrude(angle=360, convexity=2, $fn=N())
			translate([primary_engine_radius-.25,-primary_engine_cutout_depth,0])
			square([primary_engine_cutout_width,primary_engine_cutout_depth]);
		translate([primary_engine_x_offset+.5,0,primary_engine_radius*1.25])cube([2,1,1], center=true);

	}

	// SECONDARY ENGINES //
	for (i = [0:num_secondary_engines-1]){
		translate([secondary_engine_x_offset,sec_y[i],sec_z[i]]) rotate ([0,90,0])
			union() {
				cylinder(h=secondary_engine_length, r=secondary_engine_radius, center=true, $fn=N()/1.9);
				translate([0,0,-secondary_engine_length/4]) cylinder(h=secondary_engine_length*.3, r=secondary_engine_radius*1.05, center=true, $fn=N()/2);
				translate([0,0,secondary_engine_length/2]) sphere(r = secondary_engine_radius*0.7, $fn=N()/1.9);
				//translate([0,0,-2]) sphere(r = secondary_engine_radius*0.7, $fn=N());
				difference() {
					translate([0,0,secondary_engine_cone_x_offset]) scale([secondary_engine_radius,secondary_engine_radius,1]) sphere(r=1.0,$fn=N());
					translate([0,0,secondary_engine_cone_x_offset-1]) cylinder (h=2,r=secondary_engine_radius+.05,center=true,$fn=N());
					translate([0,0,secondary_engine_cone_x_offset]) scale([secondary_engine_radius,secondary_engine_radius,1]) sphere(r=0.85,$fn=N());
				}
			}
		// TRUSS
		truss_length=sqrt(sec_y[i]*sec_y[i]+sec_z[i]*sec_z[i]);
		truss_angle=atan(sec_z[i]/sec_y[i]);
		for (i=[0:3]){
			translate([truss_x_offset[i],0,0]) rotate([truss_angle+truss_angle_adjust[i],0,0]) translate([0,0,truss_length/2]) cylinder(h=truss_length,r=truss_radius,center=true,$fn=N()/3);
			translate([truss_x_offset[i],truss_y_offset[i],0]) cylinder(h=v_truss_length,r=truss_radius,center=true,$fn=N()/3);
		}
	}

	// REAR LANDING GEAR //
	for (i=[0:1]){
		translate([housing_x_offset,housing_y_offset[i],0]) cube([housing_x,housing_y,2],center=true);
	}

	// CARGO BAY //
	car_y = [cargo_bay_y_offset(),-cargo_bay_y_offset()];
	for (i = [0:1]){
		translate([0,car_y[i],cargo_bay_z_offset()]) scale([cargo_bay_length(),cargo_bay_y_radius(),cargo_bay_z_radius()]) sphere (r=1.0,$fn=N());
	}

	// FORWARD WINGS //

	translate([forward_wing_x_offset(),-.5,0])
		rotate([90,0,0])
		linear_extrude(height=forward_wing_length(), scale=forward_wing_tip_scale())
		airfoil(forward_wing_upper_axis(),forward_wing_lower_axis(),forward_upper_leading_scale(), forward_lower_scale(), N=N()/2);

	mirror([0,1,0])
		translate([forward_wing_x_offset(),-.5,0])
		rotate([90,0,0])
		linear_extrude(height=forward_wing_length(), scale=forward_wing_tip_scale())
		airfoil(forward_wing_upper_axis(),forward_wing_lower_axis(),forward_upper_leading_scale(), forward_lower_scale(), N=N()/2);

	// REAR WINGS //

	translate([rear_wing_x_offset(),-.5,0])
		rotate([90,0,0])
		linear_extrude(height=rear_wing_length(), scale=.3)
		airfoil(rear_wing_upper_axis(),rear_wing_lower_axis(),rear_upper_leading_scale(), rear_lower_scale(), N=N()/2);

	mirror([0,1,0])
		translate([rear_wing_x_offset(),-.5,0])
		rotate([90,0,0])
		linear_extrude(height=rear_wing_length(), scale=.3)
		airfoil(rear_wing_upper_axis(),rear_wing_lower_axis(),rear_upper_leading_scale(), rear_lower_scale(), N=N()/2);

	// FORWARD LANDING GEAR //

	//translate([0,0,1.7])rotate([0,20,0])forward_landing_gear(hull_length, hull_radius, N());


}
/// SUBTRACTIONS ///
translate([0,0,3]){

	// DOOR //
	translate([2,.5,.3])cube(size = [.3,1,.6], center = true);

	// BRIDGE //
	translate([bridge_forward,0,bridge_vertical]) { scale([bridge_length*bridge_interior_scale,bridge_radius*bridge_interior_scale,bridge_radius*bridge_interior_scale]) sphere (r=1.0,$fn=N());
	mirror([0,0,1]) linear_extrude(bridge_radius*.7) scale([bridge_length*bridge_interior_scale,bridge_radius*bridge_interior_scale,bridge_radius*bridge_interior_scale]) circle (r=1.0,$fn=N());
	}


	// ENGINEERING BRIDGE //
	translate([engineering_bridge_forward,0,engineering_bridge_vertical]) { scale([engineering_bridge_length*bridge_interior_scale,engineering_bridge_radius*bridge_interior_scale,engineering_bridge_radius*bridge_interior_scale]) sphere (r=1.0,$fn=N());
	mirror([0,0,1]) linear_extrude(engineering_bridge_radius*.8) scale([engineering_bridge_length*bridge_interior_scale,engineering_bridge_radius*bridge_interior_scale,engineering_bridge_radius*bridge_interior_scale]) circle (r=1.0,$fn=N());
	}

	// FORWARD LANDING GEAR SLOT //

	//forward_landing_gear(hull_length, hull_radius, N());

	// INTERIOR HALLWAY //
	translate([0,0,.4])cube([hull_length,hull_radius*.8,hull_radius*.8],center=true);
}
