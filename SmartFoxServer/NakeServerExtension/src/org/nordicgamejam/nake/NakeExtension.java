package org.nordicgamejam.nake;

import com.smartfoxserver.v2.extensions.SFSExtension;

public class NakeExtension extends SFSExtension {

	public void init()
	{
		trace("-== Nake server v0.1 loaded... ==-");
	}
	
	public void destroy()
	{
		super.destroy();
	}
}
