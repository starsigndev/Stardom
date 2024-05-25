#include "raycast.h"
#include <glm/glm.hpp>


rayCastMesh* createRCMesh() {

	return new rayCastMesh;

}

void RCAddTri(rayCastMesh* mesh, glm::vec3* p0, glm::vec3* p1, glm::vec3* p2)
{

	rayCastTri tri;
	tri.p0 = *p0;
	tri.p1 = *p1;
	tri.p2 = *p2;
	mesh->tris.push_back(tri);

}