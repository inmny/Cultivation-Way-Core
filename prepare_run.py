import os

code_dir = 'Code'

def moveToCodeDir(current_dir: str):
    children = os.listdir(current_dir)
    for child in children:
        child_path = os.path.join(current_dir, child)
        
        if os.path.isdir(child_path):
            moveToCodeDir(child_path)
        elif os.path.isfile(child_path):
            code_file_path = os.path.join(code_dir, f"LK_{current_dir.replace(os.sep, '.').replace('Code.', '')}.{child}")
            if os.path.exists(code_file_path):
                os.remove(code_file_path)
                pass
            os.link(child_path, code_file_path)
        else:
            raise Exception('Unknown file type: ' + child)

if __name__ == '__main__':
    children = os.listdir(code_dir)
    for child in children:
        child_path = os.path.join(code_dir, child)
        if os.path.isdir(child_path):
            moveToCodeDir(child_path)
        else:
            continue