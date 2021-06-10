import { System } from ".";
import { hasBody, hasPhysics } from "../components";
import { add, multiply, subtract } from "../utilities";

const frictionCoefficient = 10;

const physicsSystem: System = (game, deltaSeconds) => {
	if (!game.state.active()) return;

	for (const entity of game.entities) {
		if (!hasBody(entity) || !hasPhysics(entity)) continue;

		const frictionDecceleration = multiply(entity.velocity, frictionCoefficient);
		const totalAcceleration = subtract(entity.acceleration, frictionDecceleration);
		const deltaAcceleration = multiply(totalAcceleration, deltaSeconds);

		entity.velocity.x += deltaAcceleration.x;
		entity.velocity.y += deltaAcceleration.y;

		const deltaVelocity = multiply(entity.velocity, deltaSeconds);

		entity.position.x += deltaVelocity.x;
		entity.position.y += deltaVelocity.y;
	}
};

export { physicsSystem };
