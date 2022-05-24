use <ship_hull.scad>
module forward_landing_gear(hull_length=10, hull_radius=1,N=50){
	cubey=1;
		//union(){
		difference(){
			ship_hull(hull_length,hull_radius,N);
			mirror([0,1,0])		translate([0,cubey/2+.2,0])cube([hull_length*2,cubey,hull_radius*2],center=true);
			translate([0,cubey/2+.2,0])cube([hull_length*2,cubey,hull_radius*2],center=true);
			translate([0,0,hull_radius/2])cube([hull_length*2,1,1],center=true);
			translate([-5,0,-hull_radius/2])cube([hull_length*2,1,1],center=true);
			translate([hull_length-1,0,0])cube([2,1,2],center=true);
			scale(0.9)scale([hull_length,hull_radius,hull_radius]) sphere(r=1.0,$fn=N);
		}
}

forward_landing_gear();
