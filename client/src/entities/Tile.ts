import { Rectangle as PixiRectangle, Texture } from "pixi.js";
import { BodyComponent, createSpriteComponent, SpriteComponent, createSprite, Edges } from "../components";
import { centre, Rectangle, Vector } from "../utilities";

type Tile = BodyComponent & SpriteComponent;

const createTile = (textureAtlas: Texture, frame: Rectangle, bounds: Rectangle, edges: Edges, zIndex: number): Tile => {
	const pFrame = new PixiRectangle(frame.x, frame.y, frame.width, frame.height);
	const tileTexture: Texture = new Texture(textureAtlas.baseTexture, pFrame);

	const position: Vector = centre(bounds);
	const size: Vector = { x: bounds.width, y: bounds.height };
	const sprite: SpriteComponent = createSpriteComponent(createSprite(tileTexture, zIndex));

	return { position, size, edges, ...sprite };
};

export { createTile };