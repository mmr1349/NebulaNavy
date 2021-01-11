N=50;

rail_length=3;
rail_width=0.2;
rail_height=0.08;

barrel_length=3.5;
barrel_radius=0.1;

forward_rest_radius=.15;

cube([rail_length,rail_width,rail_height],center=true);

translate([0,0,-barrel_radius])rotate([0,90,0])cylinder(h=barrel_length,r=barrel_radius,$fn=N,center=true);

translate([0,0,-0.2])scale([5,1,1])sphere(r=forward_rest_radius,$fn=N);
