# Lesson Notes

1. Restructured code into methods instead of all inside Main().
   
2. Map is turned into a 2d array instead of array of strings.
3. While game loop to take user keyboard input.
4. Move the player character around the map, without constraints.
5. Drawing the map cell by cell including the player on the map.
6. Started a mapSettings class to store map specific info.


# Q&A  
**Q** Why is MapSettings class defined above Main() and not in a separate file?
**A** When developing on the fly sometimes its quicker and easier to create the class local to the functionality and then move it to a separate file later.