N = 50;

port_height=0.5;

max_radius=0.4;

length=3;

handle_length=1;

// muzzle device //
translate([length/1.5,0,0])rotate([0,90,0])cylinder(r=max_radius,h=length/2,$fn=N,center=true);

// barrell //
scale([length,max_radius,max_radius])sphere($fn=N);

// port //
translate([0,0,-0.2])cube([1.2,0.8,port_height],center=true);

// handle //
rotate([0,10,0])translate([-length/2,0,-handle_length])cylinder(r=max_radius/2.5,h=1,$fn=N, center=true);
