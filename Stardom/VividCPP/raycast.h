#pragma once
#include <vector>
#include <glm/glm.hpp>

extern "C" {

	struct rayCastTri {


		glm::vec3 p0, p1, p2;

	};

	struct rayCastMesh {

		std::vector<rayCastTri> tris;

	};

	__declspec(dllexport) rayCastMesh* createRCMesh();

	__declspec(dllexport) void RCAddTri(rayCastMesh* mesh, glm::vec3* p0, glm::vec3* p1, glm::vec3* p2);
}