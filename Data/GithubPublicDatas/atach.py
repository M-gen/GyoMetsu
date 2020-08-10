import glob, shutil, os

r = os.getcwd();
root_directory = r[0:len(r)-len("Project/GyoMetsu/Data/PersonalDatas")]
root_directory   = "G:/user/data_create/work_Activity/Fantasies&Foolers/"
target_directory = root_directory + "Project/GyoMetsu/GyoMetsu/bin/Debug/data"
src_directory    = root_directory + "Project/GyoMetsu/Data/GithubPublicDatas"

# ひとまず、既存のデータを削除する
files = glob.glob(target_directory+'/*')
for i in files:
    shutil.rmtree(i)

# あとはコピーするだけ
files = glob.glob(src_directory+'/*')
for i in files:
    
    print("---")
    print(i)
    name = i[i.rfind('\\')+1:]
    print(name)
    if os.path.isdir(i):
        shutil.copytree( i, target_directory+"/"+name )
    else:
        pass