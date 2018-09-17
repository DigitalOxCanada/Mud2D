# Lesson Notes

1. Create map in a file and make build process copy map to output folder in the project file.
```html
<ItemGroup>
 <None Update="maps\map1.txt">
  <CopyToOutputDirectory>Always</CopyToOutputDirectory>
 </None>
</ItemGroup>
```  

2. Read a map from a text file instead of inline.
   
3. Faster method to find where the player is located on the map.

4. Output the map to the screen.
