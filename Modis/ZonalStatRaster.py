import fiona
import pprint
from rasterstats import zonal_stats
from shapely.geometry import shape
import numpy as np
polys = ""##"E:/Documents/Google Drive/New/fiona/layers/adm1pol.shp"
raster = ""##"E:/Documents/Google Drive/New/fiona/layers/A2000049_MOLT_MOD13Q1006_B01_NDVI_3857_KZ.tif"

import sys
polys = sys.argv[1]
raster = sys.argv[2]

######################## Option №1 **************
##with fiona.open(polys) as src:
##    #for filter 
##    #features = (x for x in src if x['properties']['NameWMB_En'] == 'Esil')
##    for feat in src:
##         geom = shape(feat['geometry'])
##         id = feat['id']
##         shpId = feat['properties']['kato_te']
##         zs = zonal_stats(feat, raster, stats=['count'],all_touched=True, categorical=True) 
##         print (shpId,zs)

######################## Option №2 **************
with fiona.open(polys) as src:
    #for filter 
    #features = (x for x in src if x['properties']['NameWMB_En'] == 'Esil'')
    for feat in src:
         stats = zonal_stats(feat, raster, all_touched=True, categorical=False, geojson_out=True, stats= ['mean', 'min', 'max', 'median', 'majority', 'nodata'])
         pprint.pprint("@data$")
         pprint.pprint(str(stats[0]['properties']))

         with open("D:/file_out.txt", "a") as fout:
              fout.write(str(stats[0]['properties']) + '\n')
pprint.pprint("@data$")
########################## Option №3 ************** 
##cmap = {0: 'Missing data', 1: 'No decision', 11: 'Night', 25: 'No snow', 37: 'Lake', 39: 'Sea', 50: 'Cloud', 100: 'Lake ice', 200: 'Snow', 254: 'Detector saturated', 255: 'Fill'}
##  
##with fiona.open(polys) as src:
##     def mycount(x):
##          return np.ma.count(x)
##     for feat in src:
##         geom = shape(feat['geometry'])
##         id = feat['id']
##         shpId = feat['properties']['Id']
##         stats = shpId,zonal_stats(feat, raster, add_stats={'My count':mycount}, all_touched=True, categorical=True, category_map=cmap, stats="count")
##         pprint.pprint(stats)

##python "D:\Documents\Google Drive\New\fiona\ZonalStatRaster_v20200217v01.py" "D:/Documents/Google Drive/New/fiona/layers/adm1pol.shp" "D:/Documents/Google Drive/New/fiona/layers/A2000049_MOLT_MOD13Q1006_B01_NDVI_3857_KZ.tif"
