//// VARIABLES ////
function N() = 50;
function pos_neg() = [1,-1];

// HULL //
hull_length = 10;
hull_radius = 1;

// BRIDGE //
bridge_length = 3;
bridge_radius = 0.7;
bridge_forward = 4;
bridge_vertical = .5;

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

function wing_x_offset() = 6;

function wing_length() = 3;

function upper_leading_radius()=0.2;
function upper_leading_scale() = 3;
function upper_leading_z_offset()=[-upper_leading_radius(),upper_leading_radius()];
function upper_leading_rear_cutout_offset()=[wing_length()/2,(-wing_length())/2];

function lower_scale() = 4;

//// GENERATION CODE ////

// HULL //
scale([hull_length,hull_radius,hull_radius]) sphere(r=1.0,$fn=N());
// BRIDGE //
translate([bridge_forward,0,bridge_vertical]) scale([bridge_length,bridge_radius,bridge_radius]) sphere (r=1.0,$fn=N());
// ENGINEERING BRIDGE //
translate([engineering_bridge_forward,0,engineering_bridge_vertical]) scale([engineering_bridge_length,engineering_bridge_radius,engineering_bridge_radius]) sphere (r=1.0,$fn=N());
// PRIMARY ENGINE //
translate([primary_engine_x_offset,0,0]) rotate([0,90,0])
	union() {
		cylinder(h=primary_engine_length, r=primary_engine_radius, center=true, $fn=N());
		difference() {
			translate([0,0,primary_engine_cone_x_offset]) scale([primary_engine_radius,primary_engine_radius,1.5]) sphere(r=1.0,$fn=N());
			translate([0,0,primary_engine_cone_x_offset-1]) cylinder (h=2,r=primary_engine_radius+.05,center=true,$fn=N());
			translate([0,0,primary_engine_cone_x_offset]) scale([primary_engine_radius,primary_engine_radius,1.5]) sphere(r=0.9,$fn=N());
		}
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

rot_mat=[270,90];
yoffo=[2,-2];
for (i=[0:1]){
	translate([wing_x_offset(),yoffo[i],0])rotate([rot_mat[i],0,0])union(){
		// upper leading //
		scale([upper_leading_scale(),1]){
			difference(){

				circle(r=upper_leading_radius(),$fn=N());
				translate([0,pos_neg()[i]*upper_leading_radius()/2])square([upper_leading_radius()*2,upper_leading_radius()],center=true);
				translate([-upper_leading_radius()/2,0])square([upper_leading_radius(),upper_leading_radius()*2],center=true);
			}
		}
		scale([lower_scale(),1]){
			translate([-(lower_scale()-upper_leading_scale())*upper_leading_radius(),0]){
				difference(){
					circle(r=upper_leading_radius(),$fn=N());
					translate([0,-1*pos_neg()[i]*upper_leading_radius()/2])square([upper_leading_radius()*2,upper_leading_radius()],center=true);
				}
			}
		}
	}
}
