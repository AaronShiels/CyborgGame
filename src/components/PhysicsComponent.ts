import Vector from "../framework/Vector";
import BaseComponent from "./BaseComponent";
import { createBodyComponent } from "./BodyComponent";

interface PhysicsComponent extends BaseComponent {
	velocity: Vector;
	acceleration: Vector;
}

const hasPhysics = (object: any): object is PhysicsComponent => "velocity" in object && "acceleration" in object;
const createPhysicsComponent = (): PhysicsComponent => {
	const bodyComponent = createBodyComponent();
	return {
		...bodyComponent,
		velocity: { x: 0, y: 0 },
		acceleration: { x: 0, y: 0 },
		destroyed: false
	};
};

export default PhysicsComponent;
export { hasPhysics, createPhysicsComponent };