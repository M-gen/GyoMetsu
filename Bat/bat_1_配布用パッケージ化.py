import os, shutil, glob
import os.path
import subprocess

import datetime as dt

def CopyDataFiles():

    r = os.getcwd();
    root_directory = r[0:len(r)-len("Project/GyoMetsu/Bat")]

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
                print("copy : {0}".format(name))
            else:
                pass

    if os.path.exists(root_directory + "Project/GyoMetsu/Data/PersonalDatas"):
        CopyFiles( root_directory + "Project/GyoMetsu/GyoMetsu/bin/Release/data", root_directory + "Project/GyoMetsu/Data/PersonalDatas")
    else:
        CopyFiles( root_directory + "Project/GyoMetsu/GyoMetsu/bin/Release/data", root_directory + "Project/GyoMetsu/Data/GithubPublicDatas")

def MSBuild():
    exe = "C:/Program Files (x86)/Microsoft Visual Studio/2019/Community/MSBuild/Current/Bin/MSBuild.exe"

    # ソリューションファイルパスの算出
    root_relative_directory_path = "Bat"
    r = os.getcwd();
    root_directory = r[0:len(r)-len(root_relative_directory_path)]
    target_sln = root_directory + "GyoMetsu.sln"
    print(target_sln)

    # MSBuild
    cmd = '"{0}" "{1}" /p:Configuration=Release;Platform="Any CPU"'.format(exe, target_sln)
    p = subprocess.Popen(cmd, stdout = subprocess.PIPE, stderr = subprocess.STDOUT)
    for line in iter(p.stdout.readline,b''):
        print(line.rstrip().decode("Shift_JIS"))
        #print(line.rstrip().decode("utf8"))

def Package():
        
    def GetNewDirName( header ):
        today = dt.datetime.now()
        return header + "_" + today.strftime("%Y_%m%d_%H%M%S")

    # フォルダまるごと圧縮したかったので
    # C#でコマンドラインで圧縮する実行ファイルを作成してそれに引き渡し
    def ZipComp( zip_src, zip_dst):
        zip_exe = '"tool/ZipComp.exe"'
        cmd = zip_exe +  ' "' + zip_src + '"  "' + zip_dst + '"'
        print("----")
        print(cmd)
        print("----")
        #os.system(cmd)
            
        #cmd = "ffmpeg " + add_command
        returncode = subprocess.call(cmd, shell=True)
        print(returncode)

    # 配置
    def Setup( target_dir ):        
        os.makedirs(target_dir+'/bin', exist_ok=True)

        files = [
            'Emugen.dll', 'Emugen.pdb',
            'NAudio.dll', 'NAudio.xml',
            'OpenTK.dll', 'OpenTK.pdb', 'OpenTK.xml',

            # Roslyn系DLL
            'cs', 'de', 'es', 'fr', 'it', 'ja', 'ko', 'pl',
            'pt-BR', 'ru', 'tr', 'zh-Hans', 'zh-Hant', 
            'Microsoft.CodeAnalysis.CSharp.dll',
            'Microsoft.CodeAnalysis.CSharp.pdb',
            'Microsoft.CodeAnalysis.CSharp.Scripting.dll',
            'Microsoft.CodeAnalysis.CSharp.Scripting.pdb',
            'Microsoft.CodeAnalysis.CSharp.Scripting.xml',
            'Microsoft.CodeAnalysis.CSharp.xml',
            'Microsoft.CodeAnalysis.dll',
            'Microsoft.CodeAnalysis.pdb',
            'Microsoft.CodeAnalysis.Scripting.dll',
            'Microsoft.CodeAnalysis.Scripting.pdb',
            'Microsoft.CodeAnalysis.Scripting.xml',
            'Microsoft.CodeAnalysis.xml',
            'System.Buffers.dll',
            'System.Buffers.xml',
            'System.Collections.Immutable.dll',
            'System.Collections.Immutable.xml',
            'System.Memory.dll',
            'System.Memory.xml',
            'System.Numerics.Vectors.dll',
            'System.Numerics.Vectors.xml',
            'System.Reflection.Metadata.dll',
            'System.Reflection.Metadata.xml',
            'System.Runtime.CompilerServices.Unsafe.dll',
            'System.Runtime.CompilerServices.Unsafe.xml',
            'System.Text.Encoding.CodePages.dll',
            'System.Threading.Tasks.Extensions.dll',
            'System.Threading.Tasks.Extensions.xml',
        ]

        for i in files:
            # exists
            i2 = target_dir + "/" + i

            try:
                if os.path.isfile(i2):
                    shutil.move(i2, target_dir + '/bin/'+i)
                elif os.path.isdir(i2):
                    if os.path.isdir( target_dir + '/bin/'+i):
                        shutil.rmtree( target_dir + '/bin/'+i)
                    shutil.move(i2, target_dir + '/bin/'+i)
            except:
                pass

    project_name = "GyoMetsu"
    project_dir  = "GyoMetsu"
    target_dir   = "Release/"
    new_dir_name = GetNewDirName( target_dir + project_name+"_")
    print(new_dir_name)
    new_dir_name_zip = new_dir_name[new_dir_name.rfind("/")+1:]+".zip"

    print( "収拾開始 " + new_dir_name )

    if (os.path.exists(new_dir_name)==False):
        pass
    else:
        shutil.rmtree(new_dir_name)

    # Releaseのデータをひとまず丸ごとコピーする
    # ディレクトリがすでにあるとうまく動かないので、コレを最初に持ってくる
    shutil.copytree("../"+project_dir+"/bin/Release", new_dir_name)

    # ReadMeのコピー
    shutil.copyfile("../Document/ReadMe.txt", new_dir_name+"/ReadMe.txt")

    # 実行ファイルのリネーム
    # os.rename(main_target_dir+"TOL_SRPG.exe", main_target_dir+"TOLなSRPG.exe") # 今回は不要

    # 不要なファイルの削除
    delete_items = [
        "_setup.py",
        "FafTk.exe",
        "FafTk.exe.config",
        "FafTk.pdb",
    ]

    for i in delete_items:
        ii = new_dir_name+"/"+i
        if (os.path.exists(ii)==True):
            os.remove(ii)

    Setup(new_dir_name)

    zip_src = new_dir_name
    zip_dst = target_dir + new_dir_name_zip
    ZipComp(zip_src,zip_dst)

    print( "Zip圧縮完了 " + new_dir_name_zip )


CopyDataFiles()
MSBuild()
Package()
