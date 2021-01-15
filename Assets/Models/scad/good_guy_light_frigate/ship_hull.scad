module ship_hull(hull_length=10, hull_radius=1, N=50) {
	scale([hull_length,hull_radius,hull_radius]) sphere(r=1.0,$fn=N);
}
hull();
