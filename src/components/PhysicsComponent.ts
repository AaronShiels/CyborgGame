import { Vector } from "../shapes";
import BaseComponent from "./BaseComponent";

interface PhysicsComponent extends BaseComponent {
	velocity: Vector;
	acceleration: Vector;
}

const hasPhysics = (object: any): object is PhysicsComponent => "velocity" in object && "acceleration" in object;
export default PhysicsComponent;
export { hasPhysics };
