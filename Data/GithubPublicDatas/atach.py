import glob, shutil, os

root_relative_directory_path = "Project/GyoMetsu/Data/GithubPublicDatas"

r = os.getcwd();
root_directory = r[0:len(r)-len(root_relative_directory_path)]

print(root_directory)

def CopyFiles( target_directory, src_directory ):
    print("")
    print("---CopyFiles--- {0} {1}".format(target_directory, src_directory))

    # ひとまず、既存のデータを削除する
    files = glob.glob(target_directory+'/*')
    for i in files:
        name = i[i.rfind('\\')+1:]
        print("remove : {0}".format(name))
        shutil.rmtree(i)

    # あとはコピーするだけ
    files = glob.glob(src_directory+'/*')
    for i in files:
        
        name = i[i.rfind('\\')+1:]
        if os.path.isdir(i):
            shutil.copytree( i, target_directory+"/"+name )
            #print(i)
            print("copy : {0}".format(name))
        else:
            pass

CopyFiles( root_directory + "Project/GyoMetsu/GyoMetsu/bin/Debug/data", root_directory + root_relative_directory_path)
CopyFiles( root_directory + "Project/GyoMetsu/GyoMetsu/bin/Release/data", root_directory + root_relative_directory_path)
