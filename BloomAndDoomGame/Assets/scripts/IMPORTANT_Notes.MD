You cannot use a constructor for a MonoBehaviour like Projectile.

The reason is that you never create MonoBehaviour instances with new Projectile(). The Unity engine creates them for you when you call Instantiate(prefab) or AddComponent<Projectile>(). Since the engine handles the creation, it uses a parameterless constructor, and you don't have an opportunity to pass in arguments.

Therefore, you must use a public method (like a setter or an initialization method) to configure the projectile's properties after it has been instantiated.

A common and good practice is to create a single public Initialize method that acts like a constructor, setting up all the necessary properties at once.annot use constructors in Unity
