float Foam (float shore, float2 worldXZ, sampler2D noiseTex) {
	shore = sqrt(shore);

	float2 noiseUV = worldXZ + _Time.y * 0.25;
	float2 noise = tex2D(noiseTex, noiseUV * 0.015);

	float distortion1 = noise.x * (1 - shore);
	float foam1 = sin((shore + distortion1) * 10 - _Time.y);
	foam1 *= foam1;

	float distortion2 = noise.y * (1 - shore);
	float foam2 = sin((shore + distortion2) * 10 + _Time.y + 2);
	foam2 *= foam2 * 0.7;

	return max(foam1, foam2) * shore;
}

float Waves (float2 worldXZ, sampler2D noiseTex) {
	float2 uv1 = worldXZ;
	uv1.y += _Time.y;
	// frac vs Texture repeat??
	float4 noise1 = tex2D(noiseTex, frac(uv1 * 0.025));

	float2 uv2 = worldXZ;
	uv2.x += _Time.y;
	// frac vs Texture repeat??
	float4 noise2 = tex2D(noiseTex, frac(uv2 * 0.025));

	float blendWave = sin(
		(worldXZ.x + worldXZ.y) * 0.1 + 
		(noise1.y + noise2.z) + _Time.y
	);
	blendWave *= blendWave;

	float waves = 
		lerp(noise1.z, noise1.w, blendWave) + 
		lerp(noise2.x, noise2.y, blendWave);
	// Map [0.9;2] -> [0; 1]
	return smoothstep(0.9, 2, waves);
}