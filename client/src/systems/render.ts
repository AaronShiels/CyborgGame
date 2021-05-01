import { AnimatedSprite } from "pixi.js";
import { System } from ".";
import { AnimatedSpriteCollection, hasBody, hasSprite } from "../components";

const renderSystem: System = (game, deltaSeconds) => {
	game.stage.x = -(game.camera.x * game.stage.scale.x);
	game.stage.y = -(game.camera.y * game.stage.scale.y);

	for (const entity of game.entities) {
		if (!hasBody(entity) || !hasSprite(entity)) continue;

		if (!entity.sprite.parent) game.stage.addChild(entity.sprite);

		entity.sprite.x = Math.trunc(entity.position.x);
		entity.sprite.y = Math.trunc(entity.position.y);

		if (game.state.active() && (entity.sprite instanceof AnimatedSprite || entity.sprite instanceof AnimatedSpriteCollection))
			entity.sprite.update(deltaSeconds * 60);
	}
};

export { renderSystem };